using BusinessModel.Customer;
using CustomLoggerHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MongoConnect;
using MongoDB.Bson;
using SecretsKeyVault;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ApiDummy.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class MongoController : ControllerBase
    {        
        private IConfigurationRoot _configRoot;
        private readonly ILoggerHelper _logger;
        private readonly IMongoDbHelper<Customer> _mongoHelper;
        private readonly IMongoDbHelper<Exception> _mongoHelperEx;

        public MongoController(ILoggerHelper logger, IConfiguration configRoot
            ,IMongoDbHelper<Customer> mongoHelper, IMongoDbHelper<Exception> mongoHelperEx
            )
        {
            _logger = logger;
            _configRoot = (IConfigurationRoot)configRoot;
            _mongoHelper = mongoHelper;
            _mongoHelperEx = mongoHelperEx;
        }

        [HttpGet]
        [Route("TestPingMongo")]
        public async Task<IActionResult> TestPingMongo()
        {
            try
            {
                bool isSuccess = await _mongoHelper.TestPing();
                return Ok(isSuccess);
            }
            catch (Azure.RequestFailedException ex)
            {
                // Handle error when retrieving the secret (e.g., secret not found)
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpGet]
        [Route("GetAllCustomers")]
        public async IAsyncEnumerable<Customer> GetAllCustomers([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var cust in _mongoHelper.GetAllAsync(cancellationToken))
            {
                yield return cust;
            }
        }
        [HttpPost]
        [Route("GetCustomerByParamAsync")]
        public async Task<IActionResult> GetCustomerByParamAsync(Customer cust)
        {
            try
            {
                var result = await _mongoHelper.GetByCustomParamAsync(cust, false);
                return Ok(result);
            }
            catch (Azure.RequestFailedException ex)
            {
                // Handle error when retrieving the secret (e.g., secret not found)
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet]
        [Route("DeleteCustomerAsync")]
        public async Task<IActionResult> DeleteCustomerAsync(string id)
        {
            try
            {
                var isDeleted = await _mongoHelper.DeleteAsync(new ObjectId(id));
                return Ok(isDeleted);
            }
            catch (Azure.RequestFailedException ex)
            {
                // Handle error when retrieving the secret (e.g., secret not found)
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet]
        [Route("DeleteAllExceptionAsync")]
        public async Task<IActionResult> DeleteAllExceptionAsync()
        {
            try
            {
                var recordNum = await _mongoHelperEx.DeleteAllExceptionAsync("logs");
                return Ok(recordNum);
            }
            catch (Azure.RequestFailedException ex)
            {
                // Handle error when retrieving the secret (e.g., secret not found)
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("UpdateCustomerAsync")]
        public async Task<IActionResult> UpdateCustomerAsync(string id, Customer cust)
        {
            try
            {
                var isUpdated = await _mongoHelper.UpdateAsync(new ObjectId(id), cust);
                return Ok(isUpdated);
            }
            catch (Azure.RequestFailedException ex)
            {
                // Handle error when retrieving the secret (e.g., secret not found)
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("InsertCustomerAsync")]
        public async Task<IActionResult> InsertCustomerAsync(Customer cust)
        {
            try
            {
                var isInserted = await _mongoHelper.InsertAsync(cust);
                return Ok(isInserted);
            }
            catch (Azure.RequestFailedException ex)
            {
                // Handle error when retrieving the secret (e.g., secret not found)
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
