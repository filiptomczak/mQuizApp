using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IServices
{
    public interface IQuestionService:IBaseService<Question>
    {
        public void UpdateRange(IEnumerable<Question> questions);
    }
}
