using System.Threading.Tasks;

namespace Helpers
{
    public interface IEncryptionHelper
    {
        string EncryptStringToString_Aes(string original = "");
        string DecryptStringToString_Aes(string original = "");
        byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV);
        string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV);
    }
}
