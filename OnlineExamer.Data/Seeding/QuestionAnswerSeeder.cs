namespace OnlineExamer.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OnlineExamer.Models.Entities;

    public class QuestionAnswerSeeder : ISeeder
    {
        public void Seed(OnlineExamerDbContext context, IServiceProvider serviceProvider)
        {
            if (!context.Questions.Any())
            {
                int examId = context.Exams.FirstOrDefault().Id;

                Question[] questions =
                {
                    new Question()
                    {
                        CorrectAnswer = 1,
                        ExamId = examId,
                        Title = "asdasdasdas",
                        Answers = new HashSet<Answer>()
                        {
                            new Answer("ASDASDSADASD"),
                            new Answer("ьяаяьаьяаяьа"),
                            new Answer("ьяаяьаьяаяьа"),
                            new Answer("ьяаяьаьяаяьа"),
                        }
                    },
                    new Question()
                    {
                        CorrectAnswer = 1,
                        ExamId = examId,
                        Title = "asdasdasdas",
                        Answers = new HashSet<Answer>()
                        {
                            new Answer("asdasdasdsad"),
                            new Answer("ьяаяьаьяаяьа"),
                            new Answer("ьяаяьаьяаяьа"),
                            new Answer("ьяаяьаьяаяьа"),
                        }
                    },
                    new Question()
                    {
                        CorrectAnswer = 1,
                        ExamId = examId,
                        Title = "asdasdasdas",
                        Answers = new HashSet<Answer>()
                        {
                            new Answer("asdasdasdasd"),
                            new Answer("ьяаяьаьяаяьа"),
                            new Answer("ьяаяьаьяаяьа"),
                            new Answer("ьяаяьаьяаяьа"),
                        }
                    },
                    new Question()
                    {
                        CorrectAnswer = 1,
                        ExamId = examId,
                        Title = "asdasdasdas",
                        Answers = new HashSet<Answer>()
                        {
                            new Answer("asdsadasdsad"),
                            new Answer("ьяаяьаьяаяьа"),
                            new Answer("ьяаяьаьяаяьа"),
                            new Answer("ьяаяьаьяаяьа"),
                        }
                    },
                };

                context.Questions.AddRange(questions);
                context.SaveChanges();

                foreach (var question in context.Questions)
                {
                    question.CorrectAnswer = 1;
                }

                context.SaveChanges();
            }
        }
    }
}
