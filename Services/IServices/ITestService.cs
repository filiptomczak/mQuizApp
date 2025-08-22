using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IServices
{
    public interface ITestService
    {
        public Task<TakeTestVM> CreateTest(int quizId);
        public Task<bool> SaveResult(TestSubmissionVM model);
    }
}
