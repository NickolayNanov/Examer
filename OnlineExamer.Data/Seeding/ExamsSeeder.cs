using OnlineExamer.Models.Entities;
using OnlineExamer.Models.Entities.Enums;
using System;
using System.Linq;

namespace OnlineExamer.Data.Seeding
{
    public class ExamsSeeder : ISeeder
    {
        public void Seed(OnlineExamerDbContext context, IServiceProvider serviceProvider)
        {
            if (!context.Exams.Any())
            {
                Exam[] exams =
                {
                    new OrdinaryExam(){ ExamType = ExamType.Bulgarian, YearOfCreation = 2016 },
                    new MatriculationExam(){ ExamType = ExamType.Math, YearOfCreation = 2017 },
                    new OrdinaryExam(){ ExamType = ExamType.English, YearOfCreation = 2014 },
                    new MatriculationExam(){ ExamType = ExamType.Geography, YearOfCreation = 2015 },
                    new OrdinaryExam(){ ExamType = ExamType.History, YearOfCreation = 2017 },
                    new MatriculationExam(){ ExamType = ExamType.Psychology, YearOfCreation = 2019 },
                    new OrdinaryExam(){ ExamType = ExamType.Biology, YearOfCreation = 2013 },
                    new MatriculationExam(){ ExamType = ExamType.Math, YearOfCreation = 2018 },
                };

                context.Exams.AddRange(exams);
                context.SaveChanges();
            }
        }
    }
}
