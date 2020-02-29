namespace OnlineExamer.Models.Entities
{
    using OnlineExamer.Models.Entities.Base;

    public class Answer : BaseEntity<int>
    {
        public Answer(string content, int numberInQuestion)
        {
            this.Content = content;
            this.NumberInQuestion = numberInQuestion;
        }

        public Answer(string content)
        {
            this.Content = content;
        }

        public string Content { get; set; }

        public Question Question { get; set; }

        public int NumberInQuestion { get; set; }
    }
}
