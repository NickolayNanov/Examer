namespace OnlineExamer.Models.ViewModels.Exams
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using OnlineExamer.Models.ViewModels.Answers;
    using OnlineExamer.Models.ViewModels.Questions;

    public class ExamQuestionsViewModel
    {
        public string ExamType { get; set; }

        public int YearOfCreation { get; set; }


        public virtual string StartingMessage { get; set; }

        [Required]
        public IList<QuestionViewModel> Questions { get; set; }

        [Required]
        public IList<AnswerViewModel> Answers { get; set; }
    }
}
