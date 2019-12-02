using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using OnlineExamer.Data;
using OnlineExamer.Infrastructure;
using OnlineExamer.Models.ViewModels.Exams;

namespace OnlineExamer.Core.ExamService
{
    public class ExamService : IExamService
    {
        private readonly IMapper mapper;
        private readonly OnlineExamerDbContext context;

        public ExamService(IMapper mapper, OnlineExamerDbContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public IEnumerable<ExamViewModel> AllExams()
        {
            IEnumerable<ExamViewModel> exams = this.mapper.ProjectTo<ExamViewModel>(context.Exams).ToList();
            foreach (var exam in exams)
            {
                exam.ExamType = ExamTypeParser.Parse(exam);
            }

            return exams;
        }
    }
}
