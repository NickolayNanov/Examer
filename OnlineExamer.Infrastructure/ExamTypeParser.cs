using OnlineExamer.Models.Entities.Enums;
using OnlineExamer.Models.ViewModels.Exams;

namespace OnlineExamer.Infrastructure
{
    public static class ExamTypeParser
    {
        public static string Parse(this ExamViewModel exam)
        {
            return exam.ExamType switch
            {
                "Bulgarian" => "Български език",
                "English" => "Английски език",
                "Math" => "Математика",
                "Biology" => "Биология",
                "Psychology" => "Психология",
                "History" => "История",
                "Chemistry" => "Химия",
                "Geography" => "География",
                _ => string.Empty,
            };
        }
    }
}
