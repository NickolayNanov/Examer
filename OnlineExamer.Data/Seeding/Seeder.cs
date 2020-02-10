namespace OnlineExamer.Data.Seeding
{
    using System;
    using System.Collections.Generic;

    public static class Seeder
    {
        public static void Seed(OnlineExamerDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var seeders = new List<ISeeder>
            {
                new SchoolSubjectsSeeder(),
                new ExamsSeeder(),
                new QuestionAnswerSeeder(),
            };

            foreach (var seeder in seeders)
            {
                seeder.Seed(context);
            }
        }
    }
}
