using CustomLoggerHelper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Resiliency
{
    public interface IResiliencyHelper
    {
        Task<HttpResponseMessage> ExecuteResilientHttpRequestAsync(
            Func<Task<HttpResponseMessage>> action,
            bool useRetry = true,
            bool useCircuitBreaker = true,
            bool useTimeout = true,
            bool useFallback = false);
    }
}
