using CustomLoggerHelper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ExceptionHandlerCustom
{
    public interface IExceptionHelper
    {
        void LogException(Exception ex, ILoggerHelper logger, string additionalMessage = null);
        Task HandleExceptionAsync(Exception ex, ILoggerHelper logger);
        void HandleGlobalException(Exception ex, ILoggerHelper logger);
    }
}
