using Api.Repository.Bo.Payment;
using BusinessModel.Common;
using BusinessModel.Payment;
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
    public class PaymentController : ControllerBase
    {        
        private IConfigurationRoot _configRoot;
        private readonly ILoggerHelper _logger;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IResiliencyHelper _resiliencyHelper;
        private readonly IKeyVaultManagedIdentityHelper _secretsHelper;
        private readonly IMediator _mediator;

        public PaymentController(ILoggerHelper logger, IConfiguration configRoot
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
        [Route("GetPayments")]
        public async Task<ActionResult<PaymentBo>> GetPayments(PaymentBo request, CancellationToken cancellationToken)
        {
            var pay = await _mediator.Send(new GetPaymentRequestQuery(request), cancellationToken);
            if (pay == null)
            {
                return NotFound();
            }
            return Ok(pay);
        }

        [HttpPost]
        [Route("GetPaymentsAll")]
        public async IAsyncEnumerable<PaymentBo> GetPaymentsAll(CancellationToken cancellationToken)
        {
            var pay = _mediator.CreateStream(new GetPaymentAllRequestQuery(new PaymentBo()), cancellationToken);
            await foreach (var payment in pay.WithCancellation(cancellationToken))
            {
                yield return payment;
            }
        }
        [HttpPost]
        [Route("Pay")]
        public async Task<ActionResult<BaseResponse>> Pay(PaymentBo request, CancellationToken cancellationToken)
        {
            var pay = await _mediator.Send(new PayRequestCommand(request), cancellationToken);
            if (!pay.IsSuccess)
            {
                return StatusCode((int)pay.Status, pay);
            }
            return Ok(pay);
        }
    }
}
