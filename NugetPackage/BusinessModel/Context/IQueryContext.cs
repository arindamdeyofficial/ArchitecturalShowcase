using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BusinessModel.Context
{
    public interface IQueryContext<T> where T : DbContext
    {
        IQueryable<TEntity> Set<TEntity>() where TEntity : class;
        IQueryable<TEntity> GetAll<TEntity>() where TEntity : class;

        // Retrieve entities with a filter
        IQueryable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        // Find a single entity by key
        Task<TEntity?> FindAsync<TEntity>(params object[] keyValues) where TEntity : class;
        T GetDbContext();
        void Dispose();
    }
}
