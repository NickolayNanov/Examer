namespace OnlineExamer.Models.Entities
{
    using OnlineExamer.Models.Entities.Base;


    public class SchoolSubject : BaseEntity<int>
    {
        public SchoolSubject()
        {

        }

        public SchoolSubject(string name) : base()
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}
