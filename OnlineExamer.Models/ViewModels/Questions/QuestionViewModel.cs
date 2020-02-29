namespace OnlineExamer.Models.ViewModels.Questions
{
    using System.Collections.Generic;

    using OnlineExamer.Models.ViewModels.Answers;


    public class QuestionViewModel
    {
        public QuestionViewModel(){}
        public QuestionViewModel(string title, int correctAnswer, int selectedAnswer,int questionNumber, bool isOpenAnswer, bool isSingleAnswer, int numberInExam)
        {
            this.Title = title;
            this.IsOpenAnswer = isOpenAnswer;
            this.CorrectAnswer = correctAnswer;
            this.QuestionNumber = questionNumber;
            this.SelectedAnswer = selectedAnswer;
            this.IsSingleAnswer = isSingleAnswer;
            this.NumberInExam = numberInExam;
        }


        public string Title { get; set; }

        public bool IsOpenAnswer { get; set; }

        public int CorrectAnswer { get; set; }

        public int SelectedAnswer { get; set; }

        public int Points { get; set; }

        public bool IsSingleAnswer { get; set; }

        public int QuestionNumber { get; set; }

        public int NumberInExam { get; set; }

        public IList<AnswerViewModel> Answers { get; set; }
    }
}
