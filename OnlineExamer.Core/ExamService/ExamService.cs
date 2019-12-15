using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineExamer.Data;
using OnlineExamer.Infrastructure;
using OnlineExamer.Models.Entities;
using OnlineExamer.Models.Entities.Enums;
using OnlineExamer.Models.ViewModels.Answers;
using OnlineExamer.Models.ViewModels.Exams;
using OnlineExamer.Models.ViewModels.Questions;

namespace OnlineExamer.Core.ExamService
{
    public class ExamService : IExamService
    {
        private readonly IMapper mapper;
        private readonly OnlineExamerDbContext context;
        private readonly UserManager<OnlineExamerUser> userManager;

        public ExamService(IMapper mapper, OnlineExamerDbContext context, UserManager<OnlineExamerUser> userManager)
        {
            this.mapper = mapper;
            this.context = context;
            this.userManager = userManager;
        }

        public IEnumerable<ExamViewModel> AllExams()
        {
            IEnumerable<ExamViewModel> exams = this.mapper.ProjectTo<ExamViewModel>(context.SchoolSubjects);
            foreach (var exam in exams)
            {
                exam.ExamType = ExamTypeParser.ReverseParse(exam);
            }

            return exams;
        }

        public async Task<IEnumerable<ExamViewModel>> AllExamsByExamTypeAsync(string examType)
        {
            ExamViewModel exam = new ExamViewModel();
            exam.ExamType = examType;
            exam.ExamType = ExamTypeParser.ReverseParse(exam);

            bool doesParse = Enum.TryParse(exam.ExamType, out ExamType type);

            if (!doesParse)
            {
                return null;
            }

            IEnumerable<ExamViewModel> exams = new List<ExamViewModel>();

            await Task.Run(() =>
            {
                exams = mapper.ProjectTo<ExamViewModel>(context.Exams.Where(exam => exam.ExamType == type));
            });

            return exams;
        }

        public async Task<ExamResult> GetExamResultAsync(int examId, string username)
        {
            string userId = (await userManager.FindByNameAsync(username)).Id;
            Exam exam = await context.Exams.Include(ex => ex.Questions).SingleOrDefaultAsync(ex => ex.Id == examId);
            UserExam result = await this.context.UserExams.FirstOrDefaultAsync(ux => ux.ExamId == examId && ux.UserId == userId);
            return new ExamResult { Grade = result.Grade, MaxPoints = exam.Questions.Count, Points = result.Points };
        }

        public async Task<ExamQuestionsViewModel> LoadExamByExamTypeAndYearAsync(string examType, int year)
        {
            ExamViewModel exam = new ExamViewModel();
            exam.ExamType = examType;

            bool doesParse = Enum.TryParse(exam.ExamType, out ExamType type);

            if (!doesParse)
            {
                return null;
            }

            ExamQuestionsViewModel dto = null;
            var examm = await context.Exams
                                      .Include(x => x.Questions)
                                      .ThenInclude(x => x.Answers)
                                      .FirstOrDefaultAsync(x => x.YearOfCreation == year && x.ExamType == type);


            dto = new ExamQuestionsViewModel()
            {
                Questions = examm.Questions.Select(x => new QuestionViewModel()
                {
                    Title = x.Title,
                    IsOpenAnswer = x.IsOpenAnswer,
                    CorrectAnswer = x.CorrectAnswer,
                    Points = x.Points,
                    Answers = x.Answers.Select(a => new AnswerViewModel()
                    {
                        Content = a.Content,
                        IsSelected = false
                    }).ToList()
                }).ToList()
            };

            return dto;
        }

        public int SolveExamAsync(ExamQuestionsViewModel data, string username)
        {
            int points = 0;
            int maxPoints = data.Questions.Count;
            double grade = 0.0;

            bool doesParse = Enum.TryParse(data.ExamType, out ExamType type);

            if (!doesParse)
            {
                return 0;
            }

            OnlineExamerUser user = this.context.Users.FirstOrDefault(u => u.UserName == username);
            Exam exam = this.context.Exams.FirstOrDefault(exam => exam.ExamType == type && exam.YearOfCreation == data.YearOfCreation);

            points = CalcPoints(data);
            grade = CalcGrade(points);

            UserExam userExam = new UserExam()
            {
                Points = points,
                UserId = user.Id,
                ExamId = exam.Id,
                Grade = grade,
            };

            UserExam examFromDb = context.UserExams.FirstOrDefault(ux => ux.ExamId == exam.Id && ux.UserId == user.Id && ux.Grade >= grade);

            if (examFromDb != null)
            {
                return examFromDb.ExamId;
            }

            context.UserExams.Add(userExam);
            context.SaveChanges();

            return exam.Id;
        }

        private double CalcGrade(int points)
        {
            double grade = 0.0;
            points = 42;
            if(points < 23)
            {
                return 2.0;
            }

            grade = 3.000 + (points - 23) * 0.028;
            return grade;
        }

        private static int CalcPoints(ExamQuestionsViewModel data)
        {
            int points = 0;

            foreach (var question in data.Questions)
            {
                for (int i = 0; i < question.Answers.Count; i++)
                {
                    if (question.Answers[i].IsSelected && (i + 1) == question.CorrectAnswer)
                    {
                        points+= question.Points;
                        break;
                    }
                }
            }

            return points;
        }

        public async Task<ExamResult> GetExamResultByExamIdAndUsernameAsync(int examId, string username)
        {
            OnlineExamerUser user = await this.userManager.FindByNameAsync(username);
            Exam exam = await this.context.Exams.Include(e => e.Questions).FirstOrDefaultAsync(e => e.Id == examId);
            UserExam userExam = await this.context.UserExams.FirstOrDefaultAsync(ux => ux.ExamId == examId && ux.UserId == user.Id);

            List<UserExam> examResults = this.context.UserExams.Where(ux => ux.UserId == user.Id).Take(6).ToList();
            examResults.RemoveAt(examResults.Count - 1);

            var result = new ExamResult { Grade = userExam.Grade, Points = userExam.Points, MaxPoints = 50, ExamResultId = exam.Id };

            result.Subject = ExamTypeParser.Parse(exam);
            foreach (var item in examResults)
            {
                var currentExam = this.context.Exams.Include(e => e.Questions).FirstOrDefault(x => x.Id == item.ExamId);
                result.PastResults.Add(new ExamResult { Grade = item.Grade, Points = item.Points, MaxPoints = 50, ExamResultId = item.ExamId });
            }

            return result;
        }
    }
}
