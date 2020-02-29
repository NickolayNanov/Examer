namespace OnlineExamer.Core.AdminService
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity;

    using OnlineExamer.Data;
    using OnlineExamer.Models.ViewModels.Admin;
    using OnlineExamer.Models.ViewModels.Exams;
    using OnlineExamer.Models.Dtos.Admin;
    using OnlineExamer.Models.Entities;
    using OnlineExamer.Models.Entities.Enums;
    using OnlineExamer.Infrastructure;

    using AutoMapper;
    using System.IO;
    using OfficeOpenXml;

    public class AdminService : IAdminService
    {
        private readonly IMapper mapper;
        private readonly OnlineExamerDbContext context;
        private readonly UserManager<OnlineExamerUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AdminService(IMapper mapper, OnlineExamerDbContext context, UserManager<OnlineExamerUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.mapper = mapper;
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<ExamsAll> AllExamsAsync()
        {
            ExamsAll model = new ExamsAll();

            await Task.Run(() =>
            {
                model.Data = this.mapper.ProjectTo<ExamViewModel>(this.context.Exams.OrderBy(x => x.ExamType)).ToList();
            });

            return model;
        }

        public async Task<AdminModel> DataAsync()
        {
            AdminModel model = new AdminModel();

            await Task.Run(async () =>
            {
                string roleId = (await roleManager.FindByNameAsync("user")).Id;

                model.UsersCount = context.UserRoles.Where(x => x.RoleId == roleId).Count();
                model.ExamsCount = this.context.Exams.Count();
            });

            return model;
        }

        public async Task<bool> CreateExamAsync(ExamCreate exam)
        {
            if (string.IsNullOrEmpty(exam.ExamType) || exam.Year < 1990 || exam.Year > 2030)
            {
                return false;
            }

            string examType = ExamTypeParser.ReverseParse(exam);
            bool tryParse = Enum.TryParse(examType, out ExamType examTypeResult);

            if (!tryParse)
            {
                return false;
            }

            await CreateExamAsync(exam, examTypeResult);
            int examId = context.Exams.FirstOrDefault(ex => ex.YearOfCreation == exam.Year && ex.ExamType == examTypeResult).Id;

            List<Question> questions = new List<Question>();

            MapQuestionEntities(exam, questions);
            SetQuestionIds(exam, examId, questions);

            context.Questions.AddRange(questions);
            context.SaveChanges();

            return true;
        }

        public bool Delete(string examType, int year)
        {
            bool doesParse = Enum.TryParse(examType, out ExamType type);

            if (doesParse)
            {
                Exam exam = context.Exams
                    .Include(e => e.Questions)
                    .ThenInclude(q => q.Answers)
                    .FirstOrDefault(ex => ex.ExamType == type && ex.YearOfCreation == year);

                try
                {
                    foreach (var question in exam.Questions)
                    {
                        context.RemoveRange(question.Answers);
                    }

                    context.Questions.RemoveRange(exam.Questions);
                    context.Exams.Remove(exam);
                    context.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        public async Task<bool> CreateSubjectAsync(string subject)
        {
            if (await context.SchoolSubjects.AnyAsync(s => s.Name == subject))
            {
                return false;
            }

            SchoolSubject subjectForDb = new SchoolSubject(subject);
            context.SchoolSubjects.Add(subjectForDb);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<UserAdminViewModel> AllUsersAsync()
        {
            IList<UserViewModel> users = null;
            IList<UserViewModel> admins = null;

            await Task.Run(async () =>
            {
                string adminRole = (await this.roleManager.FindByNameAsync("admin")).Id;
                string userRole = (await this.roleManager.FindByNameAsync("user")).Id;
                string[] adminIds = context.UserRoles.Where(x => x.RoleId == adminRole).Select(x => x.UserId).ToArray();
                string[] userIds = context.UserRoles.Where(x => x.RoleId == userRole).Select(x => x.UserId).ToArray();
                users = mapper.ProjectTo<UserViewModel>(context.Users.Where(u => userIds.Contains(u.Id))).ToList();
                admins = mapper.ProjectTo<UserViewModel>(context.Users.Where(u => adminIds.Contains(u.Id))).ToList();
            });

            return new UserAdminViewModel() { Users = users, Admins = admins };
        }

        public async Task<bool> RemoveUserAsync(string username)
        {
            OnlineExamerUser user = await userManager.FindByNameAsync(username);
            context.UserExams.RemoveRange(context.UserExams.Where(ux => ux.UserId == user.Id));
            await context.SaveChangesAsync();
            return (await userManager.DeleteAsync(user)).Succeeded;
        }

        public async Task<bool> MakeAdminAsync(string username)
        {
            OnlineExamerUser user = await userManager.FindByNameAsync(username);
            await userManager.RemoveFromRoleAsync(user, "user");
            return (await userManager.AddToRoleAsync(user, "admin")).Succeeded;
        }

        public async Task<bool> RemoveFromAdminAsync(string username)
        {
            OnlineExamerUser user = await userManager.FindByNameAsync(username);
            await userManager.AddToRoleAsync(user, "user");
            return (await userManager.RemoveFromRoleAsync(user, "admin")).Succeeded;
        }

        private async Task CreateExamAsync(ExamCreate exam, ExamType examTypeResult)
        {
            await Task.Run(() =>
            {
                Exam examForDb = new Exam(examTypeResult, exam.Year);                
                context.Exams.Add(examForDb);
                context.SaveChanges();
            });
        }

        private void MapQuestionEntities(ExamCreate exam, List<Question> questions)
        {
            foreach (var question in exam.Questions)
            {
                Question q = mapper.Map<QuestionCreate, Question>(question);
                questions.Add(q);
            }
        }

        private static void SetQuestionIds(ExamCreate exam, int examId, List<Question> questions)
        {
            if(exam.Questions.Count > 0)
            {
                for (int i = 0; i < exam.Questions.Count; i++)
                {
                    AnswerCreate anser = exam.Questions[i].Answers.FirstOrDefault(a => a.IsCorrect);
                    int index = exam.Questions[i].Answers.IndexOf(anser) + 1;
                    questions[i].CorrectAnswer = index;
                    questions[i].ExamId = examId;
                }
            } 
        }

        public async Task<bool> UploadExamAsync(MemoryStream memoryStream, string subject, int year)
        {
            bool doesParse = Enum.TryParse(subject.ReverseParseStr(), out ExamType result);

            if (!doesParse)
            {
                return false;
            }

            Exam exam = await this.context.Exams.FirstOrDefaultAsync(e => e.ExamType == result && e.YearOfCreation == year);

            using (var package = new ExcelPackage(memoryStream))
            {
                ExcelWorkbook wb = package.Workbook;

                ExcelWorksheet worksheet = wb.Worksheets.FirstOrDefault();
                int index = 2;
                int questionNumberInExam = 1;
                int answerIndex = 1;
                while ((string)worksheet.Cells[index, 1].Value != null)
                {
                    string QuestionTitle = (string)worksheet.Cells[index, 1].Value;
                    string answer1 = (string)worksheet.Cells[index, 2].Value;
                    string answer2 = (string)worksheet.Cells[index, 3].Value;
                    string answer3 = (string)worksheet.Cells[index, 4].Value;
                    string answer4 = (string)worksheet.Cells[index, 5].Value;
                    int correctAnswer = 0;
                    Question question = null;

                    if (answer1 is null &&
                        answer2 is null &&
                        answer3 is null &&
                        answer4 is null)
                    {
                        correctAnswer = 5;
                        List<Answer> answers = new List<Answer> { new Answer(string.Empty), new Answer(string.Empty), new Answer(string.Empty), new Answer(string.Empty) };
                        question = new Question(correctAnswer, questionNumberInExam++, exam.Id, true, true, 0);
                        question.Title = QuestionTitle;
                        AddAnswersToQuestion(question, answers);
                    }
                    else
                    {
                        correctAnswer = (int)((double)worksheet.Cells[index, 6].Value);
                        int points = (int)((double)worksheet.Cells[index, 7].Value);
                        int single = (int)(((double)worksheet.Cells[index, 8].Value));
                        bool isSingleAnswer = single == 1 ? true : false;//if 1 then it is single answer, else it is multiple answer
                        int open = (int)(((double)worksheet.Cells[index, 9].Value));
                        bool isOpenAnswer = open == 1 ? true : false;//if 1 then it is open answer, else it is pick answer

                        question = new Question(correctAnswer, questionNumberInExam++, exam.Id, isSingleAnswer, isOpenAnswer, points);
                        question.Title = QuestionTitle;

                        List<Answer> answers = new List<Answer>();
                        answers.Add(new Answer(answer1, answerIndex++));
                        answers.Add(new Answer(answer2, answerIndex++));
                        answers.Add(new Answer(answer3, answerIndex++));
                        answers.Add(new Answer(answer4, answerIndex++));
                        answerIndex = 1;
                        AddAnswersToQuestion(question, answers);
                    }

                    this.context.Questions.Add(question);
                    index++;
                }

                this.context.SaveChanges();

                //ReadCells and parse Data
                //do something with the excel file
                package.Save();
            }

            return true;
        }

        private static void AddAnswersToQuestion(Question question, List<Answer> answers)
        {
            question.Answers.Add(answers[0]);
            question.Answers.Add(answers[1]);
            question.Answers.Add(answers[2]);
            question.Answers.Add(answers[3]);
        }
    }
}
