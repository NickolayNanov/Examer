using System.Collections.Generic;

namespace OnlineExamer.Models.Dtos.Admin
{
    public class QuestionCreate
    {
        public QuestionCreate(int points,int numberInExam, string content, List<AnswerCreate> questions, bool isSingleAnswer)
        {
            this.Points = points;
            this.Content = content;
            this.Answers = questions;
            this.IsSingleAnswer = isSingleAnswer;
            this.NumberInExam = numberInExam;
        }

        public int Points { get; set; }

        public string Content { get; set; }

        public List<AnswerCreate> Answers { get; set; }

        public bool IsSingleAnswer { get; set; }

        public int NumberInExam { get; set; }
    }
}
