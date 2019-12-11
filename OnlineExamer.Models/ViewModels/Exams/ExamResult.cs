namespace OnlineExamer.Models.ViewModels.Exams
{
    public class ExamResult
    {
        public int? ExamResultId { get; set; }

        public int Points { get; set; }

        public int MaxPoints { get; set; }

        public double Grade { get; set; }
    }
}
