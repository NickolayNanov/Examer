using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineExamer.Data;
using OnlineExamer.Infrastructure;
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

        public ExamService(IMapper mapper, OnlineExamerDbContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public IEnumerable<ExamViewModel> AllExams()
        {
            IEnumerable<ExamViewModel> exams = this.mapper.ProjectTo<ExamViewModel>(context.SchoolSubjects);
            foreach (var exam in exams)
            {
                exam.ExamType = ExamTypeParser.Parse(exam);
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

        public async Task<ExamQuestionsViewModel> LoadExamByExamTypeAndYear(string examType, int year)
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
            
            await Task.Run(() =>
            {
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
            });

            return dto;
        }
    }
}
