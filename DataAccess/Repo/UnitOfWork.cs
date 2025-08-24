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
            ISingleChoiceRepository singleChoices,
            IMatchRepository matches,
            IOpenRepository opens,
            IQuizRepository quizzes,
            IBaseRepository<Answer> answers,
            IBaseRepository<TestResult> testResults
        )
        {
            _context = context;
            Questions = questions;
            SingleChoices = singleChoices;
            Matches = matches;
            Opens = opens;
            Quizzes = quizzes;
            Answers = answers;
            TestResults = testResults;
        }

        public IQuestionRepository Questions { get; }
        public ISingleChoiceRepository SingleChoices { get; }

        public IMatchRepository Matches { get; }

        public IOpenRepository Opens { get; }

        public IQuizRepository Quizzes { get; }

        public IBaseRepository<Answer> Answers { get; }
        public IBaseRepository<TestResult> TestResults { get; }

        public async Task CommitAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}