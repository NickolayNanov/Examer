namespace OnlineExamer.Models.Entities
{
    using System;

    public class UserExam
    {
        public UserExam()
        {

        }
        public UserExam(string userId, int examId, int points, double grade)
        {
            this.UserId = userId;
            this.ExamId = examId;
            this.Points = points;
            this.Grade = grade;
        }

        public string UserId { get; set; }

        public virtual OnlineExamerUser User { get; set; }

        public int ExamId { get; set; }

        public virtual Exam Exam { get; set; }

        public double Grade { get; set; }

        public bool HasBeenStarted { get; set; }
            
        public int Points { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime? StartedAt { get; private set; }

        public DateTime? FinishedAt { get; private set; }

        public int TimesSolvedFully { get; set; }

        public void Finish()
        {
            this.FinishedAt = DateTime.Now;
        }

        public void Start()
        {
            this.FinishedAt = DateTime.Now;
        }
    }
}