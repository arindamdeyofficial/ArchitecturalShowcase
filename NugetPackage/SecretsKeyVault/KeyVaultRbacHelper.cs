using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

namespace SecretsKeyVault
{
    public class KeyVaultRbacHelper: IKeyVaultRbacHelper
    {
        private static SecretClient _secretClient;
        private static IConfigurationRoot _configRoot;
        private static string _keyVaultUrl;
        private IEnumerable<string> _secrets;
        private static IConfidentialClientApplication _confidentialClientApplication;

        public KeyVaultRbacHelper(IConfigurationRoot configRoot)
        {
            _configRoot = configRoot;
            _keyVaultUrl = _configRoot["KeyVault:Url"];            
            _secrets = new List<string>();
        }
        public async Task<string> GetSecretAsync(string secretName, IConfigurationRoot configRoot)
        {
            var cca = ConfidentialClientApplicationBuilder.Create(_configRoot["AzureAd:ClientId"])
                        .WithClientSecret(_configRoot["AzureAd:ClientSecret"])
                        .WithAuthority(new Uri($"https://login.microsoftonline.com/{_configRoot["AzureAd:TenantId"]}"))
                        .Build();
            _confidentialClientApplication = cca;
            // Get the access token using the client credentials flow
            var result = await _confidentialClientApplication.AcquireTokenForClient(
                new[] { "https://vault.azure.net/.default" })
                .ExecuteAsync();
            var token = result.AccessToken;

            // Use the token to authenticate with KeyVault
            var secretClient = new SecretClient(new Uri(_keyVaultUrl), new ClientSecretCredential(
                _configRoot["AzureAd:TenantId"],
                _configRoot["AzureAd:ClientId"],
                _configRoot["AzureAd:ClientSecret"]));

            // Retrieve the secret from Key Vault
            KeyVaultSecret secret = await secretClient.GetSecretAsync(secretName);


            // Return the secret value
            return secret.Value;
        }

    }
}
