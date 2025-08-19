using DataAccess.IRepo;
using Models.Models;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class TestResultService : BaseService<TestResult>, ITestResultService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TestResultService(
            IUnitOfWork unitOfWork) : base(unitOfWork.TestResults)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> DeleteAllByQuizIdAsync(int quizId)
        {
            var allResults = await _unitOfWork.TestResults.GetAllAsync();
            var resultsByQuizId = allResults.Where(r => r.QuizId == quizId);
            if (!resultsByQuizId.Any())
                return false;
            foreach(var result in resultsByQuizId)
            {
                _unitOfWork.TestResults.Delete(result);
            }
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
