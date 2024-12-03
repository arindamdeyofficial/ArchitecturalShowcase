using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace SecretsKeyVault
{
    public class KeyVaultManagedIdentityHelper : IKeyVaultManagedIdentityHelper
    {
        private static SecretClient _secretClient;
        private static IConfigurationRoot _configRoot;
        private IEnumerable<string> _secrets;
        private IDictionary<string, string> _secretsSource;

        public KeyVaultManagedIdentityHelper(IConfigurationRoot configRoot)
        {
            _configRoot = configRoot;
            string keyVaultUrl = _configRoot["KeyVault:Url"];
            _secretClient = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
            _secrets = new List<string>();
            _secretsSource = new Dictionary<string, string>();
        }

        public async Task<string> GetSecretAsync(string secretName)
        {
            KeyVaultSecret secret = await _secretClient.GetSecretAsync(secretName);
            return secret.Value;
        }
        public async Task<IDictionary<string, string>> GetSecretsAsync(IEnumerable<string> secretNames)
        {
            var secretTasks = secretNames.Select(async secretName =>
            {
                try
                {
                    var secret = await _secretClient.GetSecretAsync(secretName);
                    if (secret != null)
                    {
                        return new KeyValuePair<string, string>(secretName, secret.Value.ToString());
                    }
                    else
                    {
                        // Handle the case where the secret was not found
                        return new KeyValuePair<string, string>(secretName, "Secret not found");
                    }
                }
                catch (Exception ex)
                {
                    // Log the error or handle it as appropriate
                    return new KeyValuePair<string, string>(secretName, $"Error: {ex.Message}");
                }
            }).ToList();

            // Wait for all tasks to complete concurrently
            var secretResults = await Task.WhenAll(secretTasks);

            // Return the results as a dictionary
            return secretResults.ToDictionary(x => x.Key, x => x.Value);
        }
    }
}

