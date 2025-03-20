using DataAccess.IRepo;
using Models.Models;
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
            IQuestionRepository questions,
            IBaseRepository<Quiz> quizzes,
            IBaseRepository<Answer> answers
            //IBaseRepository<User> users, 
            //IBaseRepository<UserAnswer> userAnswers, 
            //IBaseRepository<UserQuiz> userQuizzes
            )
        {
            _context = context;
            Answers = answers;
            Questions = questions;
            Quizzes = quizzes;
            //Users = users;
            //UserAnswers = userAnswers;
            //UserQuizzes = userQuizzes;
        }

        //public IBaseRepository<User> Users { get; }
        //public IBaseRepository<UserAnswer> UserAnswers { get; }
        //public IBaseRepository<UserQuiz> UserQuizzes { get; }
        public IBaseRepository<Answer> Answers { get; }
        public IQuestionRepository Questions { get; }
        public IBaseRepository<Quiz> Quizzes { get; }
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
