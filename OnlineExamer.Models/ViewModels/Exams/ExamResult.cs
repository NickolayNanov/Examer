namespace OnlineExamer.Models.ViewModels.Exams
{
    using System;
    using System.Collections.Generic;

    public class ExamResult
    {
        public ExamResult()
        {
            PastResults = new List<ExamResult>();
        }

        public int? ExamId { get; set; }

        public int Points { get; set; }

        public int MaxPoints { get; set; }

        public double Grade { get; set; }

        public string Subject { get; set; }

        public int Year { get; set; }

        public DateTime SolvedOn { get; set; }

        public IEnumerable<ExamResult> PastResults { get; set; }
    }
}
