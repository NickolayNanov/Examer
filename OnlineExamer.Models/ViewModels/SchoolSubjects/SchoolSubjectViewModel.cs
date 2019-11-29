namespace OnlineExamer.Models.ViewModels.SchoolSubjects
{
    public class SchoolSubjectViewModel
    {
        public SchoolSubjectViewModel(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}
