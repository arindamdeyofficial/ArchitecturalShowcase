using CustomLoggerHelper;
using EmailConnect;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace ExceptionHandlerCustom
{
    public class ExceptionHelper : IExceptionHelper
    {
        private IConfigurationRoot _configRoot;
        private readonly ILoggerHelper _logger;
        private readonly IEmailHelper _emailHelper;
        public ExceptionHelper(ILoggerHelper logger, IConfiguration configRoot
            , IEmailHelper emailHelper)
        {
            _logger = logger;
            _configRoot = (IConfigurationRoot)configRoot;
            _emailHelper = emailHelper;
        }
        /// <summary>
        /// Logs the exception with additional custom messages.
        /// </summary>
        /// <param name="ex">The exception to be logged.</param>
        /// <param name="logger">The logger to use for logging.</param>
        /// <param name="additionalMessage">Optional additional information about the exception.</param>
        public void LogException(Exception ex, ILoggerHelper logger, string additionalMessage = null)
        {
            // Basic exception logging
            if (ex != null)
            {
                if (!string.IsNullOrEmpty(additionalMessage))
                {
                    logger.LogError(ex, $"{additionalMessage} - {ex.Message}");
                }
                else
                {
                    logger.LogError(ex, ex.Message);
                }
            }
        }

        /// <summary>
        /// Handles exceptions asynchronously, logs them, and sends an appropriate response.
        /// </summary>
        /// <param name="ex">The exception that needs to be handled.</param>
        /// <param name="logger">The logger to use for logging the exception.</param>
        public async Task HandleExceptionAsync(Exception ex, ILoggerHelper logger)
        {
            // Handle known exceptions
            if (ex is FileNotFoundException)
            {
                logger.LogError(ex, "File not found: " + ex.Message);
                // You could also send a response or take an action here (e.g., returning a 404 status code).
            }
            else if (ex is UnauthorizedAccessException)
            {
                logger.LogError(ex, "Unauthorized access: " + ex.Message);
                // Action or response for unauthorized access (e.g., returning a 403 status code).
            }
            else if (ex is ArgumentNullException)
            {
                logger.LogError(ex, "Argument is null: " + ex.Message);
                // Handle argument null exceptions here.
            }
            else
            {
                // For all other exceptions, log the error and return a generic response.
                logger.LogError(ex, $"An unexpected error occurred: {ex.Message}");
            }
            var to = "recipient@example.com";
            var subject = "Test Email";
            var body = $"<h1>This is a test email!{ex.Message}</h1>";
            await _emailHelper.SendEmailAsync(to, subject, body);
        }

        /// <summary>
        /// A global exception handler for unhandled exceptions.
        /// This can be used in middleware for ASP.NET Core applications.
        /// </summary>
        /// <param name="ex">The unhandled exception.</param>
        /// <param name="logger">The logger to use for logging the global exception.</param>
        public void HandleGlobalException(Exception ex, ILoggerHelper logger)
        {
            // Log the global exception (this is where unhandled exceptions are caught).
            if (ex != null)
            {
                logger.LogError(ex, $"Unhandled exception: {ex.Message}");
            }

            // You could also send an email alert, or log this to a centralized logging service.
            // e.g., SendEmailAlert("admin@example.com", "Unhandled Exception", ex.Message);
        }

        /// <summary>
        /// Handles HTTP-specific exceptions, returning the appropriate status codes.
        /// </summary>
        /// <param name="ex">The exception that needs to be handled.</param>
        /// <param name="logger">The logger instance.</param>
        /// <returns>A tuple containing the HTTP status code and a user-friendly message.</returns>
        public (HttpStatusCode, string) HandleHttpException(Exception ex, ILoggerHelper logger)
        {
            if (ex is HttpRequestException)
            {
                logger.LogError(ex, "Request error: " + ex.Message);
                return (HttpStatusCode.BadRequest, "There was an error with the request.");
            }

            if (ex is UnauthorizedAccessException)
            {
                logger.LogError(ex, "Unauthorized access: " + ex.Message);
                return (HttpStatusCode.Unauthorized, "You do not have permission to access this resource.");
            }

            if (ex is TimeoutException)
            {
                logger.LogError(ex, "Request timed out: " + ex.Message);
                return (HttpStatusCode.RequestTimeout, "The request has timed out. Please try again.");
            }

            // For all other exceptions, return a generic internal server error message.
            logger.LogError(ex, $"Unexpected error: {ex.Message}");
            return (HttpStatusCode.InternalServerError, "An unexpected error occurred. Please try again later.");
        }
    }
}
