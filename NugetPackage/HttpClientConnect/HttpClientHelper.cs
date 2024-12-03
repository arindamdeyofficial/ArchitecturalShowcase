using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CustomLoggerHelper;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Newtonsoft.Json;
using Resiliency;
using SecretsKeyVault;

namespace HttpClientConnect
{
    public class HttpClientHelper: IHttpClientHelper
    {
        private readonly HttpClient _httpClient;
        private IConfigurationRoot _configRoot;
        private readonly ILoggerHelper _logger;
        private readonly IResiliencyHelper _resiliencyHelper;
        private readonly Dictionary<string, string> _serviceBaseUrls;
        private readonly IKeyVaultManagedIdentityHelper _secretsHelper;

        public HttpClientHelper(ILoggerHelper logger, IConfiguration configRoot
            , HttpClient httpClient, IResiliencyHelper resiliencyHelper, IKeyVaultManagedIdentityHelper secretsHelper)
        {
            _logger = logger;
            _configRoot = (IConfigurationRoot)configRoot;
            _httpClient = httpClient;
            _serviceBaseUrls = new Dictionary<string, string>();
            _resiliencyHelper = resiliencyHelper;
            _secretsHelper = secretsHelper;
        }
        // Initialize base URLs by dynamically fetching from Key Vault
        public async Task InitializeServiceUrlsAsync()
        {
            try
            {
                // Loop through all values of HttpServiceEnum and dynamically load base URLs from Key Vault
                foreach (HttpServiceEnum serviceEnum in Enum.GetValues(typeof(HttpServiceEnum)))
                {
                    string secretKey = serviceEnum.ToString();  // Using enum name as the secret key
                    string secretValue = await _secretsHelper.GetSecretAsync(secretKey);

                    if (!string.IsNullOrEmpty(secretValue))
                    {
                        _serviceBaseUrls[serviceEnum.ToString()] = secretValue;
                    }
                    else
                    {
                        _logger.LogWarning($"No secret found for {serviceEnum}.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing service base URLs from Key Vault.");
            }
        }
        public void SetBaseUrl(HttpServiceEnum serviceEnum)
        {
            if (_serviceBaseUrls.ContainsKey(serviceEnum.ToString()))
            {
                _httpClient.BaseAddress = new Uri(_serviceBaseUrls[serviceEnum.ToString()]);
                SetDefaultHeaders("application/json", "application/json");
            }
            else
            {
                _logger.LogWarning($"Base URL for service '{serviceEnum}' not found.");
                throw new ArgumentException($"Base URL for service '{serviceEnum}' not found.");
            }
        }
        public void SetDefaultHeaders(string contentType = "application/json", string accept = "application/json")
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));
        }
        // GET method to retrieve data from the service
        public async Task<T> GetAsync<T>(HttpServiceEnum serviceEnum, string url)
        {
            try
            {
                SetBaseUrl(serviceEnum); // Set base URL dynamically
                var response = await _resiliencyHelper.ExecuteResilientHttpRequestAsync(async () =>
                {
                    return await _httpClient.GetAsync(url);
                });
                response.EnsureSuccessStatusCode(); // Throws an exception for non-success status codes
                var content = await response.Content.ReadAsStringAsync();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching data from {serviceEnum}/{url}: {ex.Message}");
                throw new ApplicationException($"Error fetching data from {serviceEnum}/{url}", ex);
            }
        }

        // POST method to send data to the service
        public async Task<T> PostAsync<T>(HttpServiceEnum serviceEnum, string url, object data)
        {
            try
            {
                SetBaseUrl(serviceEnum); // Set base URL dynamically
                var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json");
                var response = await _resiliencyHelper.ExecuteResilientHttpRequestAsync(async () =>
                {
                    return await _httpClient.PostAsync(url, content);
                });
                response.EnsureSuccessStatusCode();
                var responseData = await response.Content.ReadAsStringAsync();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error posting data to {serviceEnum}/{url}: {ex.Message}");
                throw new ApplicationException($"Error posting data to {serviceEnum}/{url}", ex);
            }
        }

        // PUT method to update data in the service
        public async Task<T> PutAsync<T>(HttpServiceEnum serviceEnum, string url, object data)
        {
            try
            {
                SetBaseUrl(serviceEnum); // Set base URL dynamically
                var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json");
                var response = await _resiliencyHelper.ExecuteResilientHttpRequestAsync(async () =>
                {
                    return await _httpClient.PutAsync(url, content);
                });
                response.EnsureSuccessStatusCode();
                var responseData = await response.Content.ReadAsStringAsync();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating data at {serviceEnum}/{url}: {ex.Message}");
                throw new ApplicationException($"Error updating data at {serviceEnum}/{url}", ex);
            }
        }

        // DELETE method to remove data from the service
        public async Task<T> DeleteAsync<T>(HttpServiceEnum serviceEnum, string url)
        {
            try
            {
                SetBaseUrl(serviceEnum); // Set base URL dynamically
                var response = await _resiliencyHelper.ExecuteResilientHttpRequestAsync(async () =>
                {
                    return await _httpClient.DeleteAsync(url);
                });
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting data at {serviceEnum}/{url}: {ex.Message}");
                throw new ApplicationException($"Error deleting data at {serviceEnum}/{url}", ex);
            }
        }
    }
}
