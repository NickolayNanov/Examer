namespace OnlineExamer.Models.Dtos.Admin
{
    using System.Collections.Generic;

    public class ExamCreate
    {
        public ExamCreate(string subject, int year, List<QuestionCreate> questions)
        {
            this.ExamType = subject;
            this.Year = year;
            this.Questions = questions;
        }

        public string ExamType { get; set; }

        public int Year { get; set; }

        public List<QuestionCreate> Questions { get; set; }
    }
}
