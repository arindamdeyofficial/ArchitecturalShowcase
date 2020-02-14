using System.Threading.Tasks;

namespace Helpers
{
    public interface IEncryptionHelper
    {
        byte[] EncryptString_Aes(string plainText, byte[] Key, byte[] IV);
        string DecryptString_Aes(byte[] cipherText, byte[] Key, byte[] IV);
        string ConvertByteArrayToStringUtf8(byte[] str);
        string ConvertByteArrayToStringAscii(byte[] str);
        byte[] ConvertStringToByteArrayUtf8(string str);
        byte[] ConvertStringToByteArrayAscii(string str);
        byte[] RemoveTraillingNullFromByteArray(byte[] bytes);
    }
}
