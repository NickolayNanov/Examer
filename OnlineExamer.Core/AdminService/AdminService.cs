namespace OnlineExamer.Core.AdminService
{
    using System.Linq;
    using System.Threading.Tasks;

    using OnlineExamer.Data;
    using OnlineExamer.Models.ViewModels.Admin;
    using OnlineExamer.Models.ViewModels.Exams;

    using AutoMapper;
    using System.Collections.Generic;
    using OnlineExamer.Models.Dtos.Admin;
    using OnlineExamer.Models.Entities;
    using System;
    using OnlineExamer.Models.Entities.Enums;
    using OnlineExamer.Infrastructure;
    using Microsoft.EntityFrameworkCore;

    public class AdminService : IAdminService
    {
        private readonly IMapper mapper;
        private readonly OnlineExamerDbContext context;

        public AdminService(IMapper mapper, OnlineExamerDbContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<ExamsAll> AllExams()
        {
            ExamsAll model = new ExamsAll();

            await Task.Run(() =>
            {
                model.Data = this.mapper.ProjectTo<ExamViewModel>(this.context.Exams.OrderBy(x => x.ExamType));                
            });

            return model;
        }

        public async Task<AdminModel> Data()
        {
            AdminModel model = new AdminModel();

            await Task.Run(() => 
            {
                model.UsersCount = this.context.Users.Count() - 1;
                model.ExamsCount = this.context.Exams.Count();
            });

            return model;
        }

        public async Task<bool> CreateExam(ExamCreate exam)
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

        public void Delete(string examType, int year)
        {
            bool doesParse = Enum.TryParse(examType, out ExamType type);

            if (doesParse)
            {
                Exam exam =  context.Exams
                    .Include(e => e.Questions)
                    .ThenInclude(q => q.Answers)
                    .FirstOrDefault(ex => ex.ExamType == type && ex.YearOfCreation == year);

                foreach (var question in exam.Questions)
                {
                    context.RemoveRange(question.Answers);
                }

                context.Questions.RemoveRange(exam.Questions);
                context.Exams.Remove(exam);
                context.SaveChanges();
            }
        }
    }
}
