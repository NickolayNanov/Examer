namespace OnlineExamer.Models.Entities
{
    using System.Collections.Generic;

    using OnlineExamer.Models.Entities.Base;

    public class Question : BaseEntity<int>
    {
        public Question() { }

        public Question(int correctAnswer, int examId, int points = 1)
        {
            this.CorrectAnswer = correctAnswer;
            this.ExamId = examId;
            this.Points = points;

            this.Answers = new HashSet<Answer>();
        }

        public Question(int correctAnswer, int numberInExam, int examId, bool isSingleAnswer, bool isOpenAnswer, int points = 1)
        {
            this.CorrectAnswer = correctAnswer;
            this.ExamId = examId;
            this.Points = points;
            this.NumberInExam = numberInExam;
            this.IsSingleAnswer = isSingleAnswer;
            this.IsOpenAnswer = isOpenAnswer;

            this.Answers = new HashSet<Answer>();
        }

        public string Title { get; set; }

        public int ExamId { get; set; }

        public Exam Exam { get; set; }

        public bool IsOpenAnswer { get; set; }

        public int CorrectAnswer { get; set; }

        public int Points { get; set; }

        public bool IsSingleAnswer { get; set; }

        public int NumberInExam { get; set; }

        public string ImgUrl { get; set; }

        public ICollection<Answer> Answers { get; set; }
    }
}
