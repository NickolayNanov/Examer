namespace OnlineExamer.Infrastructure
{
    using OnlineExamer.Models.Entities;
    using OnlineExamer.Models.ViewModels.Answers;
    using OnlineExamer.Models.ViewModels.Exams;
    using OnlineExamer.Models.ViewModels.Questions;

    using AutoMapper;
    using OnlineExamer.Models.Dtos.Admin;
    using OnlineExamer.Models.ViewModels.Admin;

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
                .ForMember(x => x.Year, y => y.MapFrom(y => y.Exam.YearOfCreation))
                .ForMember(x => x.SolvedOn, y => y.MapFrom(z => z.SolvedOn));

            this.CreateMap<AnswerCreate, Answer>()
                .ForMember(x => x.Content, y => y.MapFrom(z => z.Content));

            this.CreateMap<QuestionCreate, Question>()
                .ForMember(x => x.Title, y => y.MapFrom(z => z.Content))
                .ForMember(x => x.Points, y => y.MapFrom(z => z.Points))
                .ForMember(x => x.Answers, y => y.MapFrom(z => z.Answers));

            this.CreateMap<OnlineExamerUser, UserViewModel>()
                .ForMember(x => x.Email, y => y.MapFrom(z => z.Email))
                .ForMember(x => x.Username, y => y.MapFrom(z => z.UserName));

        }
    }
}
