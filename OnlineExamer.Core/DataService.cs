using OnlineExamer.Data;
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
            db.Questions.Add(new Models.Entities.Question() { Title = "Save" });
            var rows = await db.SaveChangesAsync();

            ;
        }
    }
}
