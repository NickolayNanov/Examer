using System.Collections.Generic;

namespace OnlineExamer.Models.Dtos.Admin
{
    public class QuestionCreate
    {
        public QuestionCreate(int points, string content, List<AnswerCreate> questions)
        {
            this.Points = points;
            this.Content = content;
            this.Answers = questions;
        }

        public int Points { get; set; }

        public string Content { get; set; }

        public List<AnswerCreate> Answers { get; set; }
    }
}
