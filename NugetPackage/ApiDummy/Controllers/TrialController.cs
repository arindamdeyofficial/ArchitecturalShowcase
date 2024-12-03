using CustomLoggerHelper;
using ExceptionHandlerCustom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MongoConnect;
using Resiliency;
using SecretsKeyVault;

namespace ApiDummy.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class TrialController : ControllerBase
    {        
        private IConfigurationRoot _configRoot;
        private readonly ILoggerHelper _logger;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IResiliencyHelper _resiliencyHelper;
        private readonly IKeyVaultManagedIdentityHelper _secretsHelper;

        public TrialController(ILoggerHelper logger, IConfiguration configRoot
            , IExceptionHelper exceptionHelper, IResiliencyHelper resiliencyHelper
            , IKeyVaultManagedIdentityHelper secretsHelper)
        {
            _logger = logger;
            _configRoot = (IConfigurationRoot)configRoot;
            _exceptionHelper = exceptionHelper;
            _resiliencyHelper = resiliencyHelper;
            _secretsHelper = secretsHelper;
        }

        [HttpGet]
        [Route("LoggerTest")]
        public void LoggerTest()
        {
            try
            {
                throw new Exception { Data = { { "Message", "Dummy Exception" }, { "ErrorCode", 1001 } } };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "test erro log");
            }
        }


        [HttpGet("{secretName}")]
        public async Task<IActionResult> GetSecretAsync(string secretName)
        {
            try
            {
                // Retrieve the secret from Azure Key Vault
                var secret = await _secretsHelper.GetSecretAsync(secretName);

                // Return the secret value
                return Ok(secret);
            }
            catch (Azure.RequestFailedException ex)
            {
                // Handle error when retrieving the secret (e.g., secret not found)
                await _exceptionHelper.HandleExceptionAsync(ex, _logger);
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
