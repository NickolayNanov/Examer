namespace OnlineExamer.Models.Entities
{
    using System.Collections.Generic;

    using OnlineExamer.Models.Entities.Base;
    using OnlineExamer.Models.Entities.Enums;

    public class Exam : BaseEntity<int>
    {
        public Exam()
        {
            this.ExamUsers = new HashSet<UserExam>();
            this.Questions = new List<Question>();
        }

        public Exam(ExamType type, int yearOfCreation)
        {
            this.ExamType = type;
            this.YearOfCreation = yearOfCreation;
        }
        
        public ExamType ExamType { get; set; }       

        public virtual string ExamStartingMessage { get; }

        public int YearOfCreation { get; set; }

        public ICollection<UserExam> ExamUsers { get; set; }

        public IList<Question> Questions { get; set; }     

        public int MaxPoints { get; set; }
    }
}
