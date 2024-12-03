using BusinessModel.Payment;
using CustomLoggerHelper;
using HttpClientConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Web.Resource;
using MongoConnect;
using MongoDB.Bson;
using SecretsKeyVault;
using SqlConnect;
using System.Transactions;

namespace ApiDummy.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class HttpClientController : ControllerBase
    {        
        private IConfigurationRoot _configRoot;
        private readonly ILoggerHelper _logger;
        private readonly IHttpClientHelper _httpClientHelper;

        public HttpClientController(ILoggerHelper logger, IConfiguration configRoot
            , IHttpClientHelper httpClientHelper
            )
        {
            _logger = logger;
            _configRoot = (IConfigurationRoot)configRoot;
            _httpClientHelper = httpClientHelper;
        }

        [HttpPost]
        [Route("CreatePayment")]
        public async Task<PaymentBo> CreatePaymentAsync(PaymentBo paymentRequest)
        {
            try
            {
                var url = "api/payments";
                var response = await _httpClientHelper.PostAsync<PaymentBo>(HttpServiceEnum.PaymentServiceBaseUrl, url, paymentRequest);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating payment.");
                throw ex;
            }
        }

        [HttpGet("{paymentId}")]
        public async Task<PaymentBo> GetPaymentStatusAsync(string transactionId)
        {
            try
            {
                var url = $"api/payments/{transactionId}";
                var response = await _httpClientHelper.GetAsync<PaymentBo>(HttpServiceEnum.PaymentServiceBaseUrl, url);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payment status.");
                throw;
            }
        }
    }
}
