using CustomLoggerHelper;
using ExceptionHandlerCustom;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using Polly;
using Polly.CircuitBreaker;
using Polly.Timeout;
using SecretsKeyVault;

namespace Resiliency
{
    public class ResiliencyHelper : IResiliencyHelper
    {
        private readonly IConfigurationRoot _configRoot;
        private readonly ILoggerHelper _logger;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IKeyVaultManagedIdentityHelper _secretsHelper;

        // Define Polly Policies
        private readonly IAsyncPolicy _retryPolicy;
        private readonly IAsyncPolicy _circuitBreakerPolicy;
        private readonly IAsyncPolicy _timeoutPolicy;
        private readonly IAsyncPolicy _fallbackPolicy;

        public ResiliencyHelper(ILoggerHelper logger, IConfiguration configRoot
            , IExceptionHelper exceptionHelper, IKeyVaultManagedIdentityHelper secretsHelper)
        {
            _logger = logger;
            _configRoot = (IConfigurationRoot)configRoot;
            _exceptionHelper = exceptionHelper;

            // Retry policy - retry 3 times with a 2-second delay between retries
            _retryPolicy = Policy.Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(2));

            // Circuit breaker policy - break the circuit after 5 failures within 30 seconds
            _circuitBreakerPolicy = Policy.Handle<HttpRequestException>()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

            // Timeout policy - throws TimeoutException if the operation doesn't complete within 5 seconds
            _timeoutPolicy = Policy.TimeoutAsync(TimeSpan.FromSeconds(5));
            _secretsHelper = secretsHelper;

            // Fallback policy - fallback to a default value when an exception occurs
            //_fallbackPolicy = Policy.Handle<Exception>().FallbackAsync(
            //    async (context, cancellationToken) =>
            //    {
            //        _logger.LogError("Fallback triggered due to failure.");
            //        return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
            //        {
            //            Content = new StringContent("Fallback response due to error.")
            //        };
            //    });
        }

        // Retry strategy example: retry action with a set number of retries
        public async Task<HttpResponseMessage> ExecuteResilientHttpRequestAsync(
            Func<Task<HttpResponseMessage>> action,
            bool useRetry = true,
            bool useCircuitBreaker = true,
            bool useTimeout = true,
            bool useFallback = false)
        {
            // Create an array to hold the policies to be applied
            var policies = new List<IAsyncPolicy>();

            var secretValue = await _secretsHelper.GetSecretAsync("ResiliencyConfigs");
            JObject jsonObject = JObject.Parse(secretValue);
            if (jsonObject["ResiliencyEnabled"].ToString() == "1")
            {      
                // Add Retry policy if required
                if (jsonObject["RetryEnabled"].ToString() == "1")
                {
                    policies.Add(_retryPolicy);
                }

                // Add Circuit Breaker policy if required
                if (jsonObject["CircuitBreakerEnabled"].ToString() == "1")
                {
                    policies.Add(_circuitBreakerPolicy);
                }

                // Add Timeout policy if required
                if (jsonObject["TimeOutEnabled"].ToString() == "1")
                {
                    policies.Add(_timeoutPolicy);
                }

                // Add Fallback policy if required
                if (jsonObject["FallBackEnabled"].ToString() == "1")
                {
                    policies.Add(_fallbackPolicy);
                }
            }

            try
            {
                if (policies.Count == 0)
                {
                    _logger.LogInformation("No resiliency policies applied, executing request normally.");
                    return await action();  // Execute the HTTP request directly
                }
                // Wrap the policies in a sequence (one policy is applied after another)
                IAsyncPolicy combinedPolicy = Policy.WrapAsync(policies.ToArray());
                // Execute the action with the combined policies
                return await combinedPolicy.ExecuteAsync(action);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Resilient HTTP request failed.");
                throw new ApplicationException("Resilient HTTP request failed", ex);
            }
        }
    }
}
