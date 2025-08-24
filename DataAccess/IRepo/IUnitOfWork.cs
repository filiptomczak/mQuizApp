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
        public IQuestionRepository Questions { get; }
        public IQuizRepository Quizzes { get; }
        public IBaseRepository<Answer> Answers { get; }
        public IBaseRepository<TestResult> TestResults { get; }

        public ISingleChoiceRepository SingleChoices { get; }
        public IMatchRepository Matches { get; }
        public IOpenRepository Opens { get; }

        public Task CommitAsync();
    }
}
