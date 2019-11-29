namespace OnlineExamer.Data.Seeding
{
    using System;
    using System.Collections.Generic;

    public static class Seeder
    {
        public static void Seed(OnlineExamerDbContext context, IServiceProvider serviceProvider)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var seeders = new List<ISeeder>
            {
                new SchoolSubjectsSeeder(),
                new ExamsSeeder(),
                new QuestionAnswerSeeder(),
            };

            foreach (var seeder in seeders)
            {
                seeder.Seed(context, serviceProvider);
            }
        }
    }
}
