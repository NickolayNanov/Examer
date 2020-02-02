namespace OnlineExamer.Models.Entities
{
    using System;

    public class UserExam
    {
        public UserExam()
        {

        }

        public UserExam(string userId, int examId, DateTime solvedOn, int points, double grade/*, string wrongAnswers*/)
        {
            this.UserId = userId;
            this.ExamId = examId;
            this.SolvedOn = solvedOn;
            this.Points = points;
            this.Grade = grade;
            //this.WrongAnswerIds = wrongAnswers;
        }

        public string UserId { get; set; }

        public OnlineExamerUser User { get; set; }

        public int ExamId { get; set; }

        public Exam Exam { get; set; }

        public double Grade { get; set; }

        public int Points { get; set; }

        public DateTime SolvedOn { get; set; }

        //public string WrongAnswerIds { get; set; }
    }
}