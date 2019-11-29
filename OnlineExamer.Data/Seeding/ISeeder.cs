using System;

namespace OnlineExamer.Data.Seeding
{
    public interface ISeeder
    {
        void Seed(OnlineExamerDbContext context, IServiceProvider serviceProvider);
    }
}
