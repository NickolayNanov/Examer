using OnlineExamer.Models.Entities.Base;
using System;

namespace OnlineExamer.Models.Entities
{
    public class UserExam : BaseEntity<int>
    {
        public string UserId { get; set; }

        public virtual OnlineExamerUser User { get; set; }

        public int ExamId { get; set; }

        public virtual Exam Exam { get; set; }

        public double Grade { get; set; }

        public bool HasBeenStarted { get; set; }
            
        public int Points { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}