using OnlineExamer.Models.Entities.Base;
using OnlineExamer.Models.Entities.Enums;
using System;
using System.Collections.Generic;

namespace OnlineExamer.Models.Entities
{
    public class Exam : BaseEntity<int>
    {
        public Exam()
        {
            this.ExamUsers = new HashSet<UserExam>();
            this.Questions = new List<Question>();
        }
        
        public ExamType ExamType { get; set; }

        public DateTime? StartedAt { get; private set; }

        public DateTime? FinishedAt { get; private set; }

        public virtual string ExamStartingMessage { get; }

        public int YearOfCreation { get; set; }

        public ICollection<UserExam> ExamUsers { get; set; }

        public IList<Question> Questions { get; set; }

        public void Finish()
        {
            this.FinishedAt = DateTime.UtcNow;
        }

        public void Start()
        {
            this.FinishedAt = DateTime.UtcNow;
        }
    }
}
