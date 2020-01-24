namespace OnlineExamer.Core.AdminService
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Microsoft.EntityFrameworkCore;

    using OnlineExamer.Data;
    using OnlineExamer.Models.ViewModels.Admin;
    using OnlineExamer.Models.ViewModels.Exams;

    using OnlineExamer.Models.Dtos.Admin;
    using OnlineExamer.Models.Entities;
    using OnlineExamer.Models.Entities.Enums;
    using OnlineExamer.Infrastructure;

    using AutoMapper;
    using Microsoft.AspNetCore.Identity;

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
            if (string.IsNullOrEmpty(exam.ExamType) || exam.Year < 1990 || exam.Year > 2030 || exam.Questions.Count == 0)
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

        private async Task CreateExamAsync(ExamCreate exam, ExamType examTypeResult)
        {
            await Task.Run(() =>
            {
                Exam examForDb = new Exam(examTypeResult, exam.Year);
                examForDb.MaxPoints = exam.Questions.Sum(q => q.Points);
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
            for (int i = 0; i < exam.Questions.Count; i++)
            {
                AnswerCreate anser = exam.Questions[i].Answers.FirstOrDefault(a => a.IsCorrect);
                int index = exam.Questions[i].Answers.IndexOf(anser) + 1;
                questions[i].CorrectAnswer = index;
                questions[i].ExamId = examId;
            }
        }

        public bool Delete(string examType, int year)
        {
            bool doesParse = Enum.TryParse(examType, out ExamType type);            

            if (doesParse)
            {
                Exam exam =  context.Exams
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

        public async Task<IList<UserViewModel>> AllUsersAsync()
        {
            IList<UserViewModel> data = null;

            await Task.Run(async () =>
            {
                IdentityRole role = await this.roleManager.FindByNameAsync("admin");
                string[] userIds = context.UserRoles.Where(x => x.RoleId != role.Id).Select(x => x.UserId).ToArray();
                data = mapper.ProjectTo<UserViewModel>(context.Users.Where(u => userIds.Contains(u.Id))).ToList();
            });

            return data;
        }

        public async Task<bool> RemoveUserAsync(string username)
        {
            OnlineExamerUser user = await userManager.FindByNameAsync(username);
            context.UserExams.RemoveRange(context.UserExams.Where(ux => ux.UserId == user.Id));
            await context.SaveChangesAsync();
            return (await userManager.DeleteAsync(user)).Succeeded;
        }

        public async Task<bool> MakeAdmin(string username)
        {
            OnlineExamerUser user = await userManager.FindByNameAsync(username);
            await userManager.RemoveFromRoleAsync(user, "user");
            return (await userManager.AddToRoleAsync(user, "admin")).Succeeded;
        }
    }
}
