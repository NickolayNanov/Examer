namespace OnlineExamer.Models.Entities
{
    public class MatriculationExam : Exam
    {
        public override string ExamStartingMessage => $"Начало на матура по {this.ExamType.ToString()} - {this.YearOfCreation} година";
    }
}
