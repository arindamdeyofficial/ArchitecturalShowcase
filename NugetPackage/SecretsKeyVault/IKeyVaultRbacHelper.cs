using Microsoft.Extensions.Configuration;

namespace SecretsKeyVault
{
    public interface IKeyVaultRbacHelper
    {
        Task<string> GetSecretAsync(string secretName, IConfigurationRoot configRoot);
    }
}
