using System;
using AutoMapper;
using AutoMapper.Configuration;
using OnlineExamer.Models.Entities;
using OnlineExamer.Models.ViewModels.Exams;
using OnlineExamer.Models.ViewModels.SchoolSubjects;

namespace OnlineExamer.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Exam, ExamViewModel>()
                .ForMember(x => x.ExamType, y => y.MapFrom(x => x.ExamType.ToString()))
                .ForMember(x => x.YearOfCreation, y => y.MapFrom(z => z.YearOfCreation))
                .ReverseMap();
        }
    }
}
