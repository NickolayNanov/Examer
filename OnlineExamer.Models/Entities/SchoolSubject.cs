namespace OnlineExamer.Models.Entities
{
    using OnlineExamer.Models.Entities.Base;


    public class SchoolSubject : BaseEntity<int>
    {
        public string Name { get; set; }
    }
}
