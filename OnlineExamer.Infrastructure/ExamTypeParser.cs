using OnlineExamer.Models.Entities;
using OnlineExamer.Models.Entities.Enums;

namespace OnlineExamer.Infrastructure
{
    public static class ExamTypeParser
    {
        public static string Parse<T>(this T exam)
        {
            var examType = (string)typeof(T).GetProperty(nameof(ExamType)).GetValue(exam).ToString();

            return examType switch
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

        public static string ReverseParse<T>(this T exam)
        {
            var examType = (string)typeof(T).GetProperty(nameof(ExamType)).GetValue(exam);

            return examType switch
            {
                "Български език" => "Bulgarian",
                "Английски език" => "English",
                "Математика" => "Math",
                "Биология" => "Biology",
                "Психология" => "Psychology",
                "История" => "History",
                "Химия" => "Chemistry",
                "География" => "Geography",
                _ => string.Empty,
            };
        }
    }
}
