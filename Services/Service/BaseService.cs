using DataAccess.IRepo;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        private readonly IBaseRepository<T> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public BaseService(IBaseRepository<T> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(T entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public void Delete(T entity)
        {
            _repository.Delete(entity);
            _unitOfWork.CommitAsync().Wait();

        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _repository.FindAsync(predicate);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            return _repository.Get(filter, includeProperties);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public void Update(T entity)
        {
            _repository.Update(entity);
            _unitOfWork.CommitAsync().Wait();
        }
    }
}
