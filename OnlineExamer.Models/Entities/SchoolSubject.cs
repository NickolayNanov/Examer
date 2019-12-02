using OnlineExamer.Models.Entities.Base;

namespace OnlineExamer.Models.Entities
{
    public class SchoolSubject : BaseEntity<int>
    {
        public string Name { get; set; }
    }
}
