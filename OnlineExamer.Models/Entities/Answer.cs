using OnlineExamer.Models.Entities.Base;

namespace OnlineExamer.Models.Entities
{
    public class Answer : BaseEntity<int>
    {
        public string Content { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
