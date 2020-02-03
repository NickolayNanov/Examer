using OnlineExamer.Models.ViewModels.Questions;
using System.Collections.Generic;

namespace OnlineExamer.Models.ViewModels.Exams
{
    public class ExamResultWithAnswers
    {
        public IList<QuestionViewModel> Questions { get; set; }

        public bool AllCorrect { get; set; }

        public string ExamType { get; set; }

        public int Year { get; set; }
    }
}
