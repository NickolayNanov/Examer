using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineExamer.Models.Entities;

namespace OnlineExamer.Data
{
    public class OnlineExamerDbContext : IdentityDbContext<OnlineExamerUser, IdentityRole, string>
    {
        public OnlineExamerDbContext(DbContextOptions<OnlineExamerDbContext> options) : base(options) { }
        public DbSet<Exam> Exams { get; set; }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<SchoolSubject> SchoolSubjects { get; set; }

        public DbSet<UserExam> UserExams { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            this.SetPrimaryKeys(builder);
            this.QuestionModelSettings(builder);
            this.AnswerModelSettings(builder);
            this.SchoolSubjectModelSettings(builder);
            this.ExamModelSettings(builder);
            UserModelSettings(builder);

            base.OnModelCreating(builder);
        }

        private static void UserModelSettings(ModelBuilder builder)
        {
            builder.Entity<OnlineExamerUser>().HasNoDiscriminator();
        }

        private void ExamModelSettings(ModelBuilder builder)
        {
            builder.Entity<Exam>()
                            .Property(x => x.YearOfCreation)
                            .IsRequired();
        }

        private void AnswerModelSettings(ModelBuilder builder)
        {
            

            builder.Entity<Answer>()
                            .Property(answer => answer.Content)
                            .IsUnicode()
                            .IsRequired();

            builder.Entity<Answer>()
                .HasOne(answer => answer.Question)
                .WithMany(question => question.Answers);
        }

        private void SchoolSubjectModelSettings(ModelBuilder builder)
        {
            builder.Entity<SchoolSubject>()
                            .Property(schoolSubject => schoolSubject.Name)
                            .IsRequired()
                            .IsUnicode();
        }

        private void QuestionModelSettings(ModelBuilder builder)
        {
            builder.Entity<Question>()
                            .Property(question => question.Title)
                            .IsUnicode()
                            .IsRequired();

            builder.Entity<Question>()
                            .HasMany(question => question.Answers)
                            .WithOne(answer => answer.Question);

            builder.Entity<Question>()
                            .HasOne(question => question.Exam)
                            .WithMany(exam => exam.Questions)
                            .HasForeignKey(fk => fk.ExamId);
        }

        private void SetPrimaryKeys(ModelBuilder builder)
        {
            builder.Entity<UserExam>().HasKey(pk => new { pk.ExamId, pk.UserId });
            builder.Entity<Question>().HasKey(pk => pk.Id);
            builder.Entity<SchoolSubject>().HasKey(pk => pk.Id);
            builder.Entity<Answer>().HasKey(pk => pk.Id);
            builder.Entity<Exam>().HasKey(pk => pk.Id);
        }
    }
}