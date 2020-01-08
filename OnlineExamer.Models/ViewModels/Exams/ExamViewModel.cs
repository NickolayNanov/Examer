namespace OnlineExamer.Models.ViewModels.Exams
{
    public class ExamViewModel
    {
        public ExamViewModel(string examType)
        {
            this.ExamType = examType;
        }

        public ExamViewModel()
        {

        }

        public string ExamType { get; set; }

        public int YearOfCreation { get; set; }
    }
}
