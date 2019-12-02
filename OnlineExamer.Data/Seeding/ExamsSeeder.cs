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
                    new OrdinaryExam(){ ExamType = ExamType.Bulgarian, YearOfCreation = 2019 },
                    new OrdinaryExam(){ ExamType = ExamType.Bulgarian, YearOfCreation = 2018 },
                    new OrdinaryExam(){ ExamType = ExamType.Bulgarian, YearOfCreation = 2017 },
                    new OrdinaryExam(){ ExamType = ExamType.Bulgarian, YearOfCreation = 2016 },
                    new OrdinaryExam(){ ExamType = ExamType.Bulgarian, YearOfCreation = 2015 },
                    new MatriculationExam(){ ExamType = ExamType.Math, YearOfCreation = 2019 },
                    new MatriculationExam(){ ExamType = ExamType.Math, YearOfCreation = 2018 },
                    new MatriculationExam(){ ExamType = ExamType.Math, YearOfCreation = 2017 },
                    new MatriculationExam(){ ExamType = ExamType.Math, YearOfCreation = 2016 },
                    new MatriculationExam(){ ExamType = ExamType.Math, YearOfCreation = 2015 },
                    new OrdinaryExam(){ ExamType = ExamType.English, YearOfCreation = 2019 },
                    new OrdinaryExam(){ ExamType = ExamType.English, YearOfCreation = 2018 },
                    new OrdinaryExam(){ ExamType = ExamType.English, YearOfCreation = 2017 },
                    new OrdinaryExam(){ ExamType = ExamType.English, YearOfCreation = 2016 },
                    new OrdinaryExam(){ ExamType = ExamType.English, YearOfCreation = 2015 },
                    new MatriculationExam(){ ExamType = ExamType.Geography, YearOfCreation = 2019 },
                    new MatriculationExam(){ ExamType = ExamType.Geography, YearOfCreation = 2018 },
                    new MatriculationExam(){ ExamType = ExamType.Geography, YearOfCreation = 2017 },
                    new MatriculationExam(){ ExamType = ExamType.Geography, YearOfCreation = 2016 },
                    new MatriculationExam(){ ExamType = ExamType.Geography, YearOfCreation = 2015 },
                    new OrdinaryExam(){ ExamType = ExamType.History, YearOfCreation = 2019 },
                    new OrdinaryExam(){ ExamType = ExamType.History, YearOfCreation = 2018 },
                    new OrdinaryExam(){ ExamType = ExamType.History, YearOfCreation = 2017 },
                    new OrdinaryExam(){ ExamType = ExamType.History, YearOfCreation = 2016 },
                    new OrdinaryExam(){ ExamType = ExamType.History, YearOfCreation = 2015 },
                    new MatriculationExam(){ ExamType = ExamType.Psychology, YearOfCreation = 2019 },
                    new MatriculationExam(){ ExamType = ExamType.Psychology, YearOfCreation = 2018 },
                    new MatriculationExam(){ ExamType = ExamType.Psychology, YearOfCreation = 2017 },
                    new MatriculationExam(){ ExamType = ExamType.Psychology, YearOfCreation = 2016 },
                    new MatriculationExam(){ ExamType = ExamType.Psychology, YearOfCreation = 2015 },
                    new OrdinaryExam(){ ExamType = ExamType.Biology, YearOfCreation = 2019 },
                    new OrdinaryExam(){ ExamType = ExamType.Biology, YearOfCreation = 2018 },
                    new OrdinaryExam(){ ExamType = ExamType.Biology, YearOfCreation = 2017 },
                    new OrdinaryExam(){ ExamType = ExamType.Biology, YearOfCreation = 2016 },
                    new OrdinaryExam(){ ExamType = ExamType.Biology, YearOfCreation = 2015 }
                };

                context.Exams.AddRange(exams);
                context.SaveChanges();
            }
        }
    }
}
