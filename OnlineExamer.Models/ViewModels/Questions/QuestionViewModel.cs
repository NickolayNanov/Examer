namespace OnlineExamer.Models.ViewModels.Questions
{
    using System.Collections.Generic;

    using OnlineExamer.Models.ViewModels.Answers;


    public class QuestionViewModel
    {
        public string Title { get; set; }

        public bool IsOpenAnswer { get; set; }

        public int CorrectAnswer { get; set; }

        public int SelectedAnswer { get; set; }

        public int Points { get; set; }

        public bool IsSingleAnswer { get; set; }

        public IList<AnswerViewModel> Answers { get; set; }
    }
}
