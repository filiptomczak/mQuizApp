using Models.Models;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepo
{
    public interface IQuizRepository : IBaseRepository<Quiz>
    {
        public Task<TakeTestVM?> GetTestVMWithQuestionsAndAnswersAsync(int id);
    }
}
