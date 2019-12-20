
namespace OnlineExamer.Core.ExamService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OnlineExamer.Models.ViewModels.Exams;

    public interface IExamService
    {
        IEnumerable<ExamViewModel> AllExams();

        Task<IEnumerable<ExamViewModel>> AllExamsByExamTypeAsync(string examType);

        Task<ExamQuestionsViewModel> LoadExamByExamTypeAndYearAsync(string examType, int year);

        int SolveExamAsync(ExamQuestionsViewModel questions, string username);

        Task<ExamResult> GetExamResultByExamIdAndUsernameAsync(int examId, string username);
    }
}
