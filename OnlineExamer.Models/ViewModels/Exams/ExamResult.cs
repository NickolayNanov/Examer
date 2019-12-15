using System.Collections.Generic;

namespace OnlineExamer.Models.ViewModels.Exams
{
    public class ExamResult
    {
        public ExamResult()
        {
            PastResults = new List<ExamResult>();
        }

        public int? ExamResultId { get; set; }

        public int Points { get; set; }

        public int MaxPoints { get; set; }

        public double Grade { get; set; }

        public string Subject { get; set; }

        public IList<ExamResult> PastResults { get; set; }
    }
}
