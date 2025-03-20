using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepo
{
    public interface IUnitOfWork
    {
        //public IBaseRepository<User> Users { get; }
        //public IBaseRepository<UserAnswer> UserAnswers { get; }
        //public IBaseRepository<UserQuiz> UserQuizzes { get; }
        //public IBaseRepository<Answer> Answers { get; }
        public IQuestionRepository Questions { get; }
        public IBaseRepository<Quiz> Quizzes { get; }
        public IBaseRepository<Answer> Answers { get; }
        public Task CommitAsync();
    }
}
