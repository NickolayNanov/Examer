
namespace OnlineExamer.Core.ExamService
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OnlineExamer.Models.ViewModels.Exams;

    public interface IExamService
    {
        IEnumerable<ExamViewModel> AllExams();

        Task<IEnumerable<ExamViewModel>> AllExamsByExamTypeAsync(string examType);

        Task<ExamQuestionsViewModel> LoadExamByExamTypeAndYearAsync(string examType, int year);

        Task<int> SolveExamAsync(ExamQuestionsViewModel questions, string username);

        Task<ExamResult> GetExamResultByExamIdAndUsernameAsync(int examId, string username);

        Task<IEnumerable<ExamResult>> GetExamResultsByUsername(string username);

        Task<ExamResultWithAnswers> GetExamResultByExamIdAndUsernameAsync(string username, int? examId, DateTime solvedOn);
    }
}
