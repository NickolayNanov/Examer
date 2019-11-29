using OnlineExamer.Models.Entities;
using System;
using System.Linq;

namespace OnlineExamer.Data.Seeding
{
    public class SchoolSubjectsSeeder : ISeeder
    {
        public void Seed(OnlineExamerDbContext context, IServiceProvider serviceProvider)
        {
            if (!context.SchoolSubjects.Any())
            {
                SchoolSubject[] schoolSubjects = new[]
                {
                    new SchoolSubject{Name = "Биология"},
                    new SchoolSubject{Name = "История"},
                    new SchoolSubject{Name = "Математика"},
                    new SchoolSubject{Name = "География"},
                    new SchoolSubject{Name = "Английски език"},
                    new SchoolSubject{Name = "Български език"},
                    new SchoolSubject{Name = "Психология"},
                };

                context.SchoolSubjects.AddRange(schoolSubjects);
                context.SaveChanges();
            }
        }
    }
}
