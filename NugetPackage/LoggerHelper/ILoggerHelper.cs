using Serilog;
using Serilog.Events;

namespace CustomLoggerHelper
{
    public interface ILoggerHelper
    {
        void LogError(Exception ex, string msg);
        void LogError(string msg);
        void LogWarning(string message);
        void LogInformation(string message);
        void LogCritical(string message);
        void CloseLogger();
    }
}
