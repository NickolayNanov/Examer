namespace OnlineExamer.Models.ViewModels.Exams
{
    using System.Collections.Generic;

    using OnlineExamer.Models.ViewModels.Questions;

    public class ExamQuestionResult
    {
        ICollection<QuestionResultViewModel> Questions { get; set; }
    }
}
