using MongoDB.Bson;
using System.Runtime.CompilerServices;

namespace MongoConnect
{
    public interface IMongoDbHelper<T>
    {
        Task<string> GetMongoConstr();
        Task<bool> TestPing();
        Task<bool> InsertAsync(T entity);
        Task<bool> UpdateAsync(ObjectId id, T updatedEntity);
        Task<bool> DeleteAsync(ObjectId id);
        Task<long> DeleteAllExceptionAsync(string collectionName);
        Task<T?> GetByIdAsync(ObjectId id);
        Task<T?> GetByCustomParamAsync(T request, bool isAnd);
        IAsyncEnumerable<T> GetAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default);

    }
}
