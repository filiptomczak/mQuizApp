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
    public class QuestionService : BaseService<QuestionBase>, IQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public QuestionService(IUnitOfWork unitOfWork) : base(unitOfWork.Questions)
        {
            _unitOfWork=unitOfWork;
        }     

        public void UpdateRange(IEnumerable<QuestionBase> questions)
        {
            _unitOfWork.Questions.UpdateRange(questions);
        }
    }
}
