using Models.Models;
using Services.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IServices
{
    public interface ITestResultService : IBaseService<TestResult>
    {
        public Task<bool> DeleteAllByQuizIdAsync(int quizId);
    }
}
