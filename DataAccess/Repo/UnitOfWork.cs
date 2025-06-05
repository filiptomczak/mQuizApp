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
            IQuizRepository quizzes,
            IBaseRepository<Answer> answers,
            IBaseRepository<TestResult> testResults
            )
        {
            _context = context;
            Answers = answers;
            Questions = questions;
            Quizzes = quizzes;
            TestResults = testResults;
        }

        public IBaseRepository<Answer> Answers { get; }
        public IQuestionRepository Questions { get; }
        public IQuizRepository Quizzes { get; }
        public IBaseRepository<TestResult> TestResults { get; }

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
