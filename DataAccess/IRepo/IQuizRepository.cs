using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepo
{
    public interface IQuizRepository : IBaseRepository<Quiz>
    {
        public Quiz GetQuizWithQuestionsAndAnswers(int id);
    }
}
