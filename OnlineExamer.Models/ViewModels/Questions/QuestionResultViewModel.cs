namespace OnlineExamer.Models.ViewModels.Questions
{
    using System.Collections.Generic;

    using OnlineExamer.Models.ViewModels.Answers;

    public class QuestionResultViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public bool IsOpenAnswer { get; set; }

        public int CorrectAnswer { get; set; }

        public int SelectedAnswer { get; set; }

        public IList<AnswerViewModel> Answers { get; set; }
    }
}
