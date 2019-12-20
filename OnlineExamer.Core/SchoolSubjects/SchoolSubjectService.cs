namespace OnlineExamer.Core.SchoolSubjects
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using OnlineExamer.Data;
    using OnlineExamer.Models.ViewModels.SchoolSubjects;

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
