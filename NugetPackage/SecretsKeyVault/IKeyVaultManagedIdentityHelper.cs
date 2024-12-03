using Microsoft.Extensions.Configuration;

namespace SecretsKeyVault
{
    public interface IKeyVaultManagedIdentityHelper
    {
        Task<string> GetSecretAsync(string secretName);
        Task<IDictionary<string, string>> GetSecretsAsync(IEnumerable<string> secretNames);
    }
}
