using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using System.Collections.ObjectModel;

namespace DataAccess
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {      
        public DbSet<Quiz>Quizzes { get; set; }
        public DbSet<QuestionBase> Questions => Set<QuestionBase>();
        public DbSet<SingleChoiceQuestion> SingleChoiceQuestions => Set<SingleChoiceQuestion>();
        public DbSet<MatchQuestion> MatchQuestions => Set<MatchQuestion>();
        public DbSet<OpenQuestion> OpenQuestions => Set<OpenQuestion>();
        public DbSet<TestResult> Results { get; set; }

        public DbSet<Answer> Answers => Set<Answer>();
        public DbSet<MatchPair> MatchPairs => Set<MatchPair>();
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {          

            base.OnModelCreating(modelBuilder);

            // Tabela Questions dla całej hierarchii
            modelBuilder.Entity<QuestionBase>().ToTable("Questions");


            // TPH discriminator
            modelBuilder.Entity<QuestionBase>()
            .HasDiscriminator<QuestionType>("QuestionType")
            .HasValue<SingleChoiceQuestion>(QuestionType.SingleChoice)
            .HasValue<MatchQuestion>(QuestionType.Match)
            .HasValue<OpenQuestion>(QuestionType.Open);


            // Relacja Quiz -> Questions (1..*)
            modelBuilder.Entity<Quiz>()
            .HasMany(q => q.Questions)
            .WithOne(qb => qb.Quiz!)
            .HasForeignKey(qb => qb.QuizId)
            .OnDelete(DeleteBehavior.Cascade);


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


            // Indeksy (opcjonalnie)
            modelBuilder.Entity<QuestionBase>()
            .HasIndex(q => q.QuizId);


            modelBuilder.Entity<Answer>()
            .HasIndex(a => a.QuestionId);


            modelBuilder.Entity<MatchPair>()
            .HasIndex(p => p.QuestionId);


        }
    }
}
