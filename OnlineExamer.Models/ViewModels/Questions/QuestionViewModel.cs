using OnlineExamer.Models.ViewModels.Answers;
using System.Collections.Generic;

namespace OnlineExamer.Models.ViewModels.Questions
{
    public class QuestionViewModel
    {
        public string Title { get; set; }

        public bool IsOpenAnswer { get; set; }

        public int CorrectAnswer { get; set; }

        public int Points { get; set; }

        public virtual IList<AnswerViewModel> Answers { get; set; }
    }
}
