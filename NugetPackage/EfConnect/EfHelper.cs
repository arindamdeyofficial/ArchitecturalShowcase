using CustomLoggerHelper;
using ExceptionHandlerCustom;
using Microsoft.Extensions.Configuration;

namespace EfConnect
{
    public class EfHelper
    {
        private IConfigurationRoot _configRoot;
        private readonly ILoggerHelper _logger;
        private readonly IExceptionHelper _exceptionHelper;
        public EfHelper(ILoggerHelper logger, IConfiguration configRoot
            , IExceptionHelper exceptionHelper)
        {
            _logger = logger;
            _configRoot = (IConfigurationRoot)configRoot;
            _exceptionHelper = exceptionHelper;
        }

    }
}
