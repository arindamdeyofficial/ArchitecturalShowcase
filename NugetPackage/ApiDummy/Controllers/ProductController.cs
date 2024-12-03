using Api.Repository.Bo.Product;
using BusinessModel.Product;
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
    public class ProductController : ControllerBase
    {        
        private IConfigurationRoot _configRoot;
        private readonly ILoggerHelper _logger;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IResiliencyHelper _resiliencyHelper;
        private readonly IKeyVaultManagedIdentityHelper _secretsHelper;
        private readonly IMediator _mediator;

        public ProductController(ILoggerHelper logger, IConfiguration configRoot
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

        [HttpPost]
        [Route("GetProductsByParam")]
        public async Task<ActionResult<ProductBo>> GetProductsByParam(ProductBo request, CancellationToken cancellationToken)
        {
            var prd = await _mediator.Send(new GetPrdRequestQuery(request), cancellationToken);
            if (prd == null)
            {
                return NotFound();
            }
            return Ok(prd);
        }

        [HttpPost]
        [Route("GetAllProducts")]
        public async IAsyncEnumerable<ProductBo> GetAllProducts(CancellationToken cancellationToken)
        {
            var prdR = _mediator.CreateStream(new GetPrdAllRequestQuery(new GetPrd()), cancellationToken);
            await foreach (var prd in prdR.WithCancellation(cancellationToken))
            {
                yield return prd;
            }
        }

        [HttpPost]
        [Route("CreatePrd")]
        public async Task<ActionResult<bool>> CreatePrd(ProductBo prd, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CreatePrdRequestQuery(prd), cancellationToken);
            return Ok(result);
        }
    }
}
