namespace OnlineExamer.Models.Dtos.Admin
{
    public class AnswerCreate
    {
        public AnswerCreate(string content, bool isCorrect)
        {
            this.Content = content;
            this.IsCorrect = isCorrect;
        }

        public string Content { get; set; }

        public bool IsCorrect { get; set; }
    }
}
