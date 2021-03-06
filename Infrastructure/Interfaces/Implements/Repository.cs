using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces.Implements
{
    public class Repository<T, TKey> : IRepository<T, TKey> where T : class
    {
        protected BeautyServiceProviderContext _dbContext;

        public Repository(BeautyServiceProviderContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> queryList = _dbContext.Set<T>().AsNoTracking();
            foreach (var expression in includes)
            {
                queryList = queryList.Include(expression);
            }
            return queryList;
        }

        public async Task<T> GetByIdAsync(TKey id)
        {
            return await _dbContext.FindAsync<T>(id);
        }

        public async Task<T> AddAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }
            await _dbContext.AddAsync(entity);
            return entity;
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }
            _dbContext.Update(entity);

        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }
            _dbContext.Remove(entity);
        }

        public void DeleteRange(Expression<Func<T, bool>> predicate)
        {
            var lstRemove = _dbContext.Set<T>().Where(predicate);
            _dbContext.RemoveRange(lstRemove);
        }
    }
}
