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
    public class QuestionService : BaseService<Question>, IQuestionService
    {
        private readonly IQuestionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public QuestionService(IQuestionRepository repository, IUnitOfWork unitOfWork) : base(repository,unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void UpdateRange(IEnumerable<Question> questions)
        {
            _repository.UpdateRange(questions);
            _unitOfWork.CommitAsync().Wait();
        }
    }
}
