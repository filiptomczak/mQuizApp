using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepo
{
    public interface IQuestionRepository : IBaseRepository<QuestionBase>
    {
        public void UpdateRange(IEnumerable<QuestionBase> questions);
        public Task<IEnumerable<QuestionBase>> GetByQuizIdAsync(int quizId);
    }
}
