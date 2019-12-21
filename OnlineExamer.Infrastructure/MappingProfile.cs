namespace OnlineExamer.Infrastructure
{
    using OnlineExamer.Models.Entities;
    using OnlineExamer.Models.ViewModels.Answers;
    using OnlineExamer.Models.ViewModels.Exams;
    using OnlineExamer.Models.ViewModels.Questions;

    using AutoMapper;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Exam, ExamViewModel>()
                .ForMember(x => x.ExamType, y => y.MapFrom(x => x.ExamType.ToString()))
                .ForMember(x => x.YearOfCreation, y => y.MapFrom(z => z.YearOfCreation))
                .ReverseMap();

            this.CreateMap<SchoolSubject, ExamViewModel>()
                .ForMember(x => x.ExamType, y => y.MapFrom(z => z.Name));

            this.CreateMap<Answer, AnswerViewModel>();
            this.CreateMap<Question, QuestionViewModel>();

            this.CreateMap<Exam, ExamQuestionsViewModel>()
                .ForMember(x => x.Questions, y => y.MapFrom(z => z.Questions))
                .ForMember(x => x.ExamType, y => y.MapFrom(z => z.ExamType.ToString()));

            this.CreateMap<UserExam, ExamResult>()
                .ForMember(x => x.ExamResultId, y => y.MapFrom(z => z.ExamId))
                .ForMember(x => x.Points, y => y.MapFrom(z => z.Points))
                .ForMember(x => x.Grade, y => y.MapFrom(y => y.Grade))
                .ForMember(x => x.Subject, y => y.MapFrom(y => y.Exam.Parse()))
                .ForMember(x => x.Year, y => y.MapFrom(y => y.Exam.YearOfCreation));
        }
    }
}
