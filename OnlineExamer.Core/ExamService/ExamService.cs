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
    using System.Text;
    using OnlineExamer.Models.Dtos.Others;

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

            examm.Questions = examm.Questions.OrderBy(q => q.NumberInExam).ToList();

            foreach (var question in examm.Questions)
            {
                question.Answers = question.Answers.OrderBy(a => a.NumberInQuestion).ToList();
            }


            dto = new ExamQuestionsViewModel()
            {
                Questions = examm.Questions.Select(x => new QuestionViewModel()
                {
                    Title = x.Title,
                    IsOpenAnswer = x.IsOpenAnswer,
                    CorrectAnswer = x.CorrectAnswer,
                    Points = x.Points,
                    IsSingleAnswer = x.IsSingleAnswer,
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
                MaxPoints = userExam.MaxPoints,
                ExamId = exam.Id,
                Subject = ExamTypeParser.Parse(exam),
                PastResults = FillPastResults(examResults)
            };

            return result;
        }

        public async Task<int> SolveExamAsync(ExamQuestionsViewModel data, string username)
        {
            double grade = 0.0;
            bool doesParse = Enum.TryParse(data.ExamType, out ExamType type);

            if (!doesParse) return 0;

            OnlineExamerUser user = await this.context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            Exam exam = await context.Exams.FirstOrDefaultAsync(e => e.YearOfCreation == data.YearOfCreation && e.ExamType == type);

            ExamPoinsAnswers examResult = CalcExamResults(data);
            grade = CalcGrade(examResult.Points);

            UserExam userExam = new UserExam(user.Id, exam.Id, DateTime.Now, examResult.Points, grade, examResult.WrongAnswers);
            userExam.MaxPoints = data.Questions.Count;
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

        private ExamPoinsAnswers CalcExamResults(ExamQuestionsViewModel data)
        {
            ExamPoinsAnswers examResult = new ExamPoinsAnswers();
            StringBuilder sb = new StringBuilder();

            foreach (var question in data.Questions)
            {
                for (int i = 0; i < question.Answers.Count; i++)
                {
                    if (question.Answers[i].IsSelected && (i + 1) == question.CorrectAnswer)
                    {
                        examResult.Points += question.Points;
                        break;
                    }
                    if (question.Answers[i].IsSelected && (i + 1) != question.CorrectAnswer)
                    {
                        sb.Append($"{data.Questions.IndexOf(question)} - {i + 1}, ");
                        break;
                    }
                }
            }

            examResult.WrongAnswers = sb.ToString();
            return examResult;
        }

        private IEnumerable<ExamResult> FillPastResults(List<UserExam> examResults)
        {
            foreach (UserExam examResult in examResults)
            {
                yield return new ExamResult { Grade = examResult.Grade, Points = examResult.Points, MaxPoints = examResult.MaxPoints, ExamId = examResult.ExamId };
            }
        }

        public async Task<ExamResultWithAnswers> GetExamResultByExamIdAndUsernameAsync(string username, int? examId, DateTime solvedOn)
        {
            ExamResultWithAnswers result = new ExamResultWithAnswers();

            if (examId.HasValue)
            {
                OnlineExamerUser user = await userManager.FindByNameAsync(username);
                UserExam ux = await this.context.UserExams
                    .Include(x => x.Exam)
                        .ThenInclude(x => x.Questions)
                            .ThenInclude(x => x.Answers)
                    .FirstOrDefaultAsync(x => x.ExamId == examId.Value && x.UserId == user.Id && x.SolvedOn == solvedOn);

                result.Year = ux.Exam.YearOfCreation;
                result.ExamType = ux.Exam.Parse();

                if (string.IsNullOrEmpty(ux.WrongAnswerIds))
                {
                    result.AllCorrect = true;
                }
                else
                {
                    string[] questionsAnswers = ux.WrongAnswerIds.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                    var questions = ux.Exam.Questions.ToList();

                    foreach (var questionAnswer in questionsAnswers)
                    {
                        string[] tokens = questionAnswer.Split(new char[] { ' ', ',', '-' }, StringSplitOptions.RemoveEmptyEntries);
                        int questionId = int.Parse(tokens[0]);
                        int answer = int.Parse(tokens[1]);

                        Question currentQuestion = questions[questionId];
                        if (currentQuestion.Title.StartsWith("ТЕКСТ")) continue;
                        QuestionViewModel question = new QuestionViewModel(currentQuestion.Title, 
                                                                           currentQuestion.CorrectAnswer, 
                                                                           answer, 
                                                                           questionId + 1,
                                                                           currentQuestion.IsOpenAnswer, 
                                                                           currentQuestion.IsSingleAnswer,
                                                                           currentQuestion.NumberInExam);

                        question.Answers = currentQuestion.Answers.Select(x => mapper.Map<AnswerViewModel>(x)).ToList();
                        result.Questions.Add(question);
                    }
                    result.Questions = result.Questions.OrderBy(x => x.NumberInExam).ToList();
                    result.Questions.ToList().ForEach(q => q.Answers = q.Answers.OrderBy(a => a.NumberInQuestion).ToList());
                }
            }

            return result;
        }
    }
}
