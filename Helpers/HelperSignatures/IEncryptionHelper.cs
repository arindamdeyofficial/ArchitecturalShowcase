using System.Threading.Tasks;

namespace Helpers
{
    public interface IEncryptionHelper
    {
        EncryptionDetails EncryptString_Aes(string plainText);
        EncryptionDetails DecryptString_Aes(EncryptionDetails cipherDetails);
    }
}
