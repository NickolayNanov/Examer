namespace OnlineExamer.Core.AdminService
{
    using System.Threading.Tasks;

    using OnlineExamer.Models.Dtos.Admin;
    using OnlineExamer.Models.ViewModels.Admin;
    using OnlineExamer.Models.ViewModels.Exams;

    public interface IAdminService
    {
        Task<AdminModel> Data();

        Task<ExamsAll> AllExams();

        Task<bool> CreateExam(ExamCreate questions);

        void Delete(string examType, int year);
    }  
}
