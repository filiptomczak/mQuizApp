using Microsoft.EntityFrameworkCore;
using Models.Models;
using System.Collections.ObjectModel;

namespace DataAccess
{
    public class AppDbContext : DbContext
    {
        //public DbSet<Answer>Answers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Quiz>Quizzes { get; set; }
        //public DbSet<User>Users { get; set; }
        //public DbSet<UserAnswer>UserAnswers{ get; set; }
        //public DbSet<UserQuiz>UserQuizzes{ get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {          

            base.OnModelCreating(modelBuilder);
            var q1 = new Question
            {
                Id = 1,
                Text = "Pytanie 1",
                QuizId = 1,
            };
            var q2 = new Question
            {
                Id = 2,
                Text = "Pytanie 2",
                QuizId = 1,
            };
            var q3 = new Question
            {
                Id = 3,
                Text = "Pytanie 3",
                QuizId = 1,
            };
            modelBuilder.Entity<Question>().HasData(q1, q2, q3);
            modelBuilder.Entity<Quiz>().HasData(
                new Quiz
                {
                    Id = 1,
                    Title = "Test wiedzy ogólnej",
                    Description = "Sprawdzamy wiedzę ogólną",
                });

        }
    }
}
