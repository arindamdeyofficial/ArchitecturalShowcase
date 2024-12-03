using CustomLoggerHelper;
using ExceptionHandlerCustom;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Resiliency;
using SecretsKeyVault;

namespace ApiDummy.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class HomeController : ControllerBase
    {        
        private IConfigurationRoot _configRoot;
        private readonly ILoggerHelper _logger;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IResiliencyHelper _resiliencyHelper;
        private readonly IKeyVaultManagedIdentityHelper _secretsHelper;
        private readonly IMediator _mediator;

        public HomeController(ILoggerHelper logger, IConfiguration configRoot
            , IExceptionHelper exceptionHelper, IMediator mediator)
        {
            _logger = logger;
            _configRoot = (IConfigurationRoot)configRoot;
            _exceptionHelper = exceptionHelper;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("Deploytime")]
        public string? Deploytime()
        {
            var deployTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            return deployTime;
        }
    }
}
