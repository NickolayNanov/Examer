namespace OnlineExamer.Core.SchoolSubjects
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OnlineExamer.Models.ViewModels.SchoolSubjects;

    public interface ISchoolSubjectService
    {
        Task<IEnumerable<SchoolSubjectViewModel>> GetAll();
    }
}
