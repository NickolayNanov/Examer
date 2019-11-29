namespace OnlineExamer.Models.Entities
{
    public class OrdinaryExam : Exam
    {
        public override string ExamStartingMessage => $"Начало на тест по {this.ExamType.ToString()}";
    }
}
