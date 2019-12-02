using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using OnlineExamer.Data;
using OnlineExamer.Models.ViewModels.SchoolSubjects;

namespace OnlineExamer.Core.SchoolSubjects
{
    public class SchoolSubjectService : ISchoolSubjectService
    {
        private readonly OnlineExamerDbContext context;

        public SchoolSubjectService(OnlineExamerDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<SchoolSubjectViewModel>> GetAll()
        {
            IEnumerable<SchoolSubjectViewModel> schoolSubjects = new List<SchoolSubjectViewModel>();

            await Task.Run(() =>
            {
                schoolSubjects = context.SchoolSubjects.Select(x => new SchoolSubjectViewModel(x.Name)).AsEnumerable();
            });

            return schoolSubjects;
        }
    }
}
