using System.Configuration;
using System.Reflection.Metadata;

namespace BusinessModel.Interface.Common
{
    public interface IBaseRequest<T, U>
    {
        Task<U> Execute(T request, CancellationToken cancellationToken);
    }
}
