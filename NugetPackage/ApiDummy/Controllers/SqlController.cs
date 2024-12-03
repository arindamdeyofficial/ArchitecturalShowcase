using BusinessModel;
using CustomLoggerHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Web.Resource;
using MongoConnect;
using MongoDB.Bson;
using SecretsKeyVault;
using SqlConnect;

namespace ApiDummy.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class SqlController : ControllerBase
    {        
        private IConfigurationRoot _configRoot;
        private readonly ILoggerHelper _logger;
        private readonly ISqlHelper _sqlHelper;

        public SqlController(ILoggerHelper logger, IConfiguration configRoot
            , ISqlHelper sqlHelper
            )
        {
            _logger = logger;
            _configRoot = (IConfigurationRoot)configRoot;
            _sqlHelper = sqlHelper;
        }

        [HttpGet]
        [Route("TestSql")]
        public async Task<IActionResult> TestSql()
        {
            // Example of Insert (Adding a new payment record)
            var insertPaymentQuery = "INSERT INTO Payments (PaymentAmount, PaymentDate, CustomerId, PaymentMethod) VALUES (@PaymentAmount, @PaymentDate, @CustomerId, @PaymentMethod)";
            var insertPaymentParameters = new SqlParameter[]
            {
                new SqlParameter("@PaymentAmount", 100.00),
                new SqlParameter("@PaymentDate", DateTime.Now),
                new SqlParameter("@CustomerId", 123),
                new SqlParameter("@PaymentMethod", "CreditCard")
            };
            await _sqlHelper.InsertAsync(insertPaymentQuery, insertPaymentParameters, SqlDbEnum.DeviceRentalDbConStr);

            // Example of Update (Updating an existing payment status)
            var updatePaymentQuery = "UPDATE Payments SET PaymentStatus = @PaymentStatus WHERE PaymentId = @PaymentId";
            var updatePaymentParameters = new SqlParameter[]
            {
                new SqlParameter("@PaymentStatus", "Completed"),
                new SqlParameter("@PaymentId", 456)
            };
            await _sqlHelper.UpdateAsync(updatePaymentQuery, updatePaymentParameters, SqlDbEnum.DeviceRentalDbConStr);

            // Example of Select (Fetching payment details)
            var selectPaymentQuery = "SELECT * FROM Payments WHERE CustomerId = @CustomerId";
            var selectPaymentParameters = new SqlParameter[]
            {
                new SqlParameter("@CustomerId", 123)
            };
            var paymentRecords = await _sqlHelper.SelectAsync(selectPaymentQuery, selectPaymentParameters, SqlDbEnum.DeviceRentalDbConStr);

            // Example of Stored Procedure (Executing a stored procedure for payment processing)
            var paymentProcedureParameters = new SqlParameter[]
            {
                new SqlParameter("@CustomerId", 123),
                new SqlParameter("@PaymentAmount", 100.00),
                new SqlParameter("@PaymentMethod", "CreditCard"),
                new SqlParameter("@TransactionId", DBNull.Value)  // Example of output parameter
            };
            await _sqlHelper.ExecuteStoredProcedureAsync("sp_ProcessPayment", paymentProcedureParameters, SqlDbEnum.DeviceRentalDbConStr);

            return Ok();
        }
    }
}
