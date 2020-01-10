namespace OnlineExamer.Core.ExamService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using OnlineExamer.Data;
    using OnlineExamer.Infrastructure;
    using OnlineExamer.Models.Entities;
    using OnlineExamer.Models.Entities.Enums;
    using OnlineExamer.Models.ViewModels.Answers;
    using OnlineExamer.Models.ViewModels.Exams;
    using OnlineExamer.Models.ViewModels.Questions;

    using AutoMapper;

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
            ExamViewModel exam = new ExamViewModel() { ExamType = examType };            
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
            Exam examm = await context.Exams
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

        public async Task<ExamResult> GetExamResultByExamIdAndUsernameAsync(int examId, string username)
        {
            OnlineExamerUser user = await userManager.FindByNameAsync(username);
            Exam exam = await context.Exams.FirstOrDefaultAsync(e => e.Id == examId);
            UserExam userExam = context.UserExams.Where(ux => ux.ExamId == examId && ux.UserId == user.Id).ToList().Last();
            List<UserExam> examResults = context.UserExams
                                                    .Include(x => x.Exam)
                                                    .Where(ux => ux.UserId == user.Id && ux.Exam.ExamType == exam.ExamType)
                                                    .Take(5)
                                                    .ToList();

            if (examResults.Count == 1)
            {
                examResults.RemoveAt(0);
            }
            else
            {
                examResults.RemoveAt(examResults.Count - 1);
            }

            ExamResult result = new ExamResult
            {
                Grade = userExam.Grade,
                Points = userExam.Points,
                MaxPoints = exam.MaxPoints,
                ExamResultId = exam.Id,
                Subject = ExamTypeParser.Parse(exam),
                PastResults = FillPastResults(examResults)
            };

            return result;
        }

        public async Task<int> SolveExamAsync(ExamQuestionsViewModel data, string username)
        {
            int points = 0;
            double grade = 0.0;

            bool doesParse = Enum.TryParse(data.ExamType, out ExamType type);

            if (!doesParse)
            {
                return 0;
            }

            OnlineExamerUser user = await this.context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            Exam exam = await context.Exams.FirstOrDefaultAsync(e => e.YearOfCreation == data.YearOfCreation && e.ExamType == type);

            points = CalcPoints(data);
            grade = CalcGrade(points);

            UserExam userExam = new UserExam(user.Id, exam.Id, DateTime.Now, points, grade);

            context.UserExams.Add(userExam);
            context.SaveChanges();

            return exam.Id;
        }

        public async Task<IEnumerable<ExamResult>> GetExamResultsByUsername(string username)
        {
            OnlineExamerUser user = await userManager.FindByNameAsync(username);
            IEnumerable<ExamResult> result = this.mapper
                                                    .ProjectTo<ExamResult>(context.UserExams.Include(x => x.Exam)
                                                        .Where(ux => ux.UserId == user.Id))
                                                    .ToList()
                                                    .OrderByDescending(e => e.SolvedOn);


            return result;
        }

        //TODO: Make use of a grade
        private double CalcGrade(int points)
        {
            if (points < 23)
            {
                return 2.0;
            }

            return 3.000 + (points - 23) * 0.028;
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
                        points += question.Points;
                        break;
                    }
                }
            }

            return points;
        }

        private IEnumerable<ExamResult> FillPastResults(List<UserExam> examResults)
        {
            foreach (UserExam examResult in examResults)
            {
                yield return new ExamResult { Grade = examResult.Grade, Points = examResult.Points, MaxPoints = examResult.Exam.MaxPoints, ExamResultId = examResult.ExamId };
            }
        }
    }
}
