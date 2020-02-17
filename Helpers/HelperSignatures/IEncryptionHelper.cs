using System.Threading.Tasks;

namespace Helpers
{
    public interface IEncryptionHelper
    {
        EncryptionDetails EncryptString_Aes(string plainText);
        EncryptionDetails DecryptString_Aes(EncryptionDetails cipherDetails);
        string ConvertByteArrayToStringUtf8(byte[] str);
        string ConvertByteArrayToStringAscii(byte[] str);
        byte[] ConvertStringToByteArrayUtf8(string str);
        byte[] ConvertStringToByteArrayAscii(string str);
        byte[] RemoveTraillingNullFromByteArray(byte[] bytes);
    }
}
