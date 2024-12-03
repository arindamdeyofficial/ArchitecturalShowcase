using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BusinessModel.Context
{
    public class QueryContext<T> : IQueryContext<T> where T : DbContext
    {
        private readonly BaseContext<T> _baseContext;
        private readonly DbContext _dbContext;
        public QueryContext(BaseContext<T> baseContext)
        {
            _baseContext = baseContext;
            _dbContext = _baseContext.GetDbContext();
        }

        public IQueryable<TEntity> Set<TEntity>() where TEntity : class
        {
            return _dbContext.Set<TEntity>();
        }

        // Retrieve all entities
        public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return _dbContext.Set<TEntity>();
        }

        // Retrieve entities with a filter
        public IQueryable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return _dbContext.Set<TEntity>().Where(predicate);
        }

        // Find a single entity by key
        public async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues) where TEntity : class
        {
            return await _dbContext.Set<TEntity>().FindAsync(keyValues);
        }
        public T GetDbContext()
        {
            return (T)_dbContext;
        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
