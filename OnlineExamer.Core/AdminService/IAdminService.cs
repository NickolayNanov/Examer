namespace OnlineExamer.Core.AdminService
{
    using System.IO;
    using System.Threading.Tasks;

    using OnlineExamer.Models.Dtos.Admin;
    using OnlineExamer.Models.ViewModels.Admin;
    using OnlineExamer.Models.ViewModels.Exams;

    public interface IAdminService
    {
        Task<AdminModel> DataAsync();

        Task<ExamsAll> AllExamsAsync();

        Task<bool> CreateExamAsync(ExamCreate questions);

        bool Delete(string examType, int year);

        Task<bool> CreateSubjectAsync(string subject);

        Task<UserAdminViewModel> AllUsersAsync();

        Task<bool> RemoveUserAsync(string username);

        Task<bool> MakeAdminAsync(string username);

        Task<bool> RemoveFromAdminAsync(string username);

        Task<bool> UploadExamAsync(MemoryStream memoryStream, string subject, int Year);
    }  
}
