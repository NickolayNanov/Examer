namespace OnlineExamer.Infrastructure
{
    using OnlineExamer.Models.Entities.Enums;

    public static class ExamTypeParser
    {
        public static string Parse<T>(this T exam)
        {
            string examType = typeof(T).GetProperty(nameof(ExamType)).GetValue(exam).ToString();

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
                "French" => "Френски",
                "Spanish" => "Испански",
                "Deutsche" => "Немски",
                _ => string.Empty,
            };
        }

        public static string ReverseParse<T>(this T exam)
        {
            string examType = typeof(T).GetProperty(nameof(ExamType)).GetValue(exam).ToString();

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
                "Френски" => "French",
                "Испански" => "Spanish",
                "Немски" => "Deutsche",
                _ => string.Empty,
            };
        }

        public static string ReverseParseStr(this string examTypeStr)
        {
            return examTypeStr switch
            {
                "Български език" => "Bulgarian",
                "Английски език" => "English",
                "Математика" => "Math",
                "Биология" => "Biology",
                "Психология" => "Psychology",
                "История" => "History",
                "Химия" => "Chemistry",
                "География" => "Geography",
                "Френски" => "French",
                "Испански" => "Spanish",
                "Немски" => "Deutsche",
                _ => string.Empty,
            };
        }

        public static string ParseStr(this string examTypeStr)
        {
            return examTypeStr switch
            {
                "Bulgarian" => "Български език",
                "English" => "Английски език",
                "Math" => "Математика",
                "Biology" => "Биология",
                "Psychology" => "Психология",
                "History" => "История",
                "Chemistry" => "Химия",
                "Geography" => "География",
                "French" => "Френски",
                "Spanish" => "Испански",
                "Deutsche" => "Немски",
                _ => string.Empty
            };
        }
    }
}
