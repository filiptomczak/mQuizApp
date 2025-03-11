using DataAccess.IRepo;
using QuizApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repo
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(
            AppDbContext context, 
            IBaseRepository<Answer> answers, 
            IBaseRepository<Question> questions, 
            IBaseRepository<Quiz> quizzes, 
            IBaseRepository<User> users, 
            IBaseRepository<UserAnswer> userAnswers, 
            IBaseRepository<UserQuiz> userQuizzes)
        {
            _context = context;
            Answers = answers;
            Questions = questions;
            Quizzes = quizzes;
            Users = users;
            UserAnswers = userAnswers;
            UserQuizzes = userQuizzes;
        }

        public IBaseRepository<Answer> Answers { get; }
        public IBaseRepository<Question> Questions { get; }
        public IBaseRepository<Quiz> Quizzes { get; }
        public IBaseRepository<User> Users { get; }
        public IBaseRepository<UserAnswer> UserAnswers { get; }
        public IBaseRepository<UserQuiz> UserQuizzes { get; }
        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
