using OnlineExamer.Models.Entities.Base;

namespace OnlineExamer.Models.Entities
{
    public class Answer : BaseEntity<int>
    {
        public Answer(string content)
        {
            this.Content = content;
        }

        public string Content { get; set; }

        public Question Question { get; set; }
    }
}
