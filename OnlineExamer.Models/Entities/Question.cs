namespace OnlineExamer.Models.Entities
{
    using System.Collections.Generic;

    using OnlineExamer.Models.Entities.Base;

    public class Question : BaseEntity<int>
    {
        public Question()
        {
            this.Answers = new HashSet<Answer>();
        }

        public string Title { get; set; }

        public int ExamId { get; set; }

        public Exam Exam { get; set; }

        public bool IsOpenAnswer { get; set; }

        public int CorrectAnswer { get; set; }

        public int Points { get; set; }

        public ICollection<Answer> Answers { get; set; }
    }
}
