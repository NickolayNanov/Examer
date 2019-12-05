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

        public async Task<IEnumerable<ExamViewModel>> AllExamsByExamType(string examType)
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

        public async Task<ExamResults> GetExamResult(int examResultId)
        {
            var result = await this.context.UserExams.FirstOrDefaultAsync(ux => ux.Id == examResultId);
            return new ExamResults { Grade = result.Grade, MaxPoints = 4, Points = result.Points };
        }

        public ExamQuestionsViewModel LoadExamByExamTypeAndYear(string examType, int year)
        {
            ExamViewModel exam = new ExamViewModel();
            exam.ExamType = examType;

            bool doesParse = Enum.TryParse(exam.ExamType, out ExamType type);

            if (!doesParse)
            {
                return null;
            }

            ExamQuestionsViewModel dto = null;
            var examm = context.Exams.Include(x => x.Questions).ThenInclude(x => x.Answers).FirstOrDefault(x => x.YearOfCreation == year && x.ExamType == type);


            dto = new ExamQuestionsViewModel()
            {
                Questions = examm.Questions.Select(x => new QuestionViewModel()
                {
                    Title = x.Title,
                    IsOpenAnswer = x.IsOpenAnswer,
                    CorrectAnswer = x.CorrectAnswer,
                    Answers = x.Answers.Select(a => new AnswerViewModel()
                    {
                        Content = a.Content,
                        IsSelected = false
                    }).ToList()
                }).ToList()
            };

            return dto;
        }

        public async Task<int> SolveExam(ExamQuestionsViewModel data, string username)
        {
            int points = 0;
            int maxPoints = data.Questions.Count;

            bool doesParse = Enum.TryParse(data.ExamType, out ExamType type);

            if (!doesParse)
            {
                return 0;
            }

            OnlineExamerUser user = await this.userManager.FindByNameAsync(username);
            Exam exam = this.context.Exams.FirstOrDefault(exam => exam.ExamType == type && exam.YearOfCreation == data.YearOfCreation);

            foreach (var question in data.Questions)
            {
                for (int i = 0; i < question.Answers.Count; i++)
                {
                    if (question.Answers[i].IsSelected && (i + 1) == question.CorrectAnswer)
                    {
                        points++;
                        break;
                    }
                }
            }

            UserExam userExam = new UserExam()
            {
                Points = points,
                UserId = user.Id,
                ExamId = exam.Id,
                Grade = 6,
            };

            context.UserExams.Add(userExam);
            context.SaveChanges();

            return this.context.UserExams.FirstOrDefault(x => x.UserId == user.Id && x.ExamId == exam.Id).Id;
        }
    }
}
