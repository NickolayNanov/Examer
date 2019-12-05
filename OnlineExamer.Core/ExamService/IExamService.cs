using OnlineExamer.Models.ViewModels.Exams;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineExamer.Core.ExamService
{
    public interface IExamService
    {
        IEnumerable<ExamViewModel> AllExams();

        Task<IEnumerable<ExamViewModel>> AllExamsByExamType(string examType);

        ExamQuestionsViewModel LoadExamByExamTypeAndYear(string examType, int year);

        Task<int> SolveExam(ExamQuestionsViewModel questions, string username);

        Task<ExamResults> GetExamResult(int examResultId);
    }
}
