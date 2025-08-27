using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Enums;
using Models.Models;
using System.Collections.ObjectModel;

namespace DataAccess
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<QuestionBase> Questions =>Set<QuestionBase>();
        public DbSet<SingleChoiceQuestion> SingleChoiceQuestions => Set<SingleChoiceQuestion>();
        public DbSet<MatchQuestion> MatchQuestions => Set<MatchQuestion>();
        public DbSet<OpenQuestion> OpenQuestions => Set<OpenQuestion>();

        public DbSet<Answer> Answers => Set<Answer>();
        public DbSet<MatchPair> MatchPairs => Set<MatchPair>();

        public DbSet<Quiz>Quizzes => Set<Quiz>();
        public DbSet<TestResult> Results => Set<TestResult>();
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {          

            base.OnModelCreating(modelBuilder);

            //table questions dla całech hierarchii
            modelBuilder.Entity<QuestionBase>().ToTable("Questions");

            // TPH discriminator
            modelBuilder.Entity<QuestionBase>()
                .HasDiscriminator<QuestionType>("QuestionType")
                .HasValue<SingleChoiceQuestion>(QuestionType.SingleChoice)
                .HasValue<MatchQuestion>(QuestionType.Match)
                .HasValue<OpenQuestion>(QuestionType.Open);

            // Relacja SingleChoiceQuestion -> Answers (1..*)
            modelBuilder.Entity<SingleChoiceQuestion>()
                .HasMany(sc => sc.Answers)
                .WithOne(a => a.Question!)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacja MatchQuestion -> MatchPairs (1..*)
            modelBuilder.Entity<MatchQuestion>()
                .HasMany(mq => mq.Pairs)
                .WithOne(p => p.Question!)
                .HasForeignKey(p => p.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
            
            //var a1 = new Answer
            //{
            //    Id=1,
            //    QuestionId = 1,
            //    Text = "Odp1",
            //    IsCorrect = true,
            //};
            //var a2 = new Answer
            //{
            //    Id = 2,
            //    QuestionId = 1,
            //    Text = "Odp2",
            //    IsCorrect = false,
            //};

            var q1 = new SingleChoiceQuestion
            {
                Id = 1,
                Text = "Pytanie 1",
                QuizId = 1,
                //Answers = new List<Answer> { a1,a2},
            };       

            var quiz1= new Quiz
            {
                Id = 1,
                Title = "Test wiedzy ogólnej",
                Description = "Sprawdzamy wiedzę ogólną",
            };


            //modelBuilder.Entity<Answer>().HasData(a1, a2);
            modelBuilder.Entity<SingleChoiceQuestion>().HasData(q1);
            modelBuilder.Entity<Quiz>().HasData(quiz1);
                
        }
    }
}
