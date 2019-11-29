using OnlineExamer.Data;
using OnlineExamer.Models.Entities;
using System.Threading.Tasks;

namespace OnlineExamer.Core
{
    public class DataService
    {
        private readonly OnlineExamerDbContext db;

        public DataService(OnlineExamerDbContext db)
        {
            this.db = db;
        }

        public async Task Save()
        {
            var d = db;
            var question = new Exam() { ExamType = Models.Entities.Enums.ExamType.Biology, YearOfCreation = 2014, };
            db.Exams.Add(question);
            var rows = await db.SaveChangesAsync();
        }
    }
}
