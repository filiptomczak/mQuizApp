using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepo
{
    public interface IQuestionRepository:IBaseRepository<Question>
    {
        public void UpdateRange(IEnumerable<Question> questions);
    }
}
