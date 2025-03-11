using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace DataAccess
{
    public class AppDbContext : DbContext
    {
        public DbSet<Answer>Answers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Quiz>Quizzes { get; set; }
        public DbSet<User>Users { get; set; }
        public DbSet<UserAnswer>UserAnswers{ get; set; }
        public DbSet<UserQuiz>UserQuizzes{ get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
