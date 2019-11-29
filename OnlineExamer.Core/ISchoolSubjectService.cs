using OnlineExamer.Models.ViewModels.SchoolSubjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineExamer.Core
{
    public interface ISchoolSubjectService
    {
        Task<IEnumerable<SchoolSubjectViewModel>> GetAll();
    }
}
