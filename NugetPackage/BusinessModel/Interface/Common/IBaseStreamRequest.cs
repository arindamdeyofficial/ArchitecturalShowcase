using System.Configuration;
using System.Reflection.Metadata;

namespace BusinessModel.Interface.Common
{
    public interface IBaseStreamRequest<T, U>
    {
        IAsyncEnumerable<U> ExecuteStreamQueryAsync(T request, CancellationToken cancellationToken);
    }
}
