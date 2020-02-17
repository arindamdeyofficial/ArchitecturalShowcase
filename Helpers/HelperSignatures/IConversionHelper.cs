using System.Threading.Tasks;

namespace Helpers
{
    public interface IConversionHelper
    {
        string ConvertByteArrayToStringUtf8(byte[] str);
        string ConvertByteArrayToStringAscii(byte[] str);
        byte[] ConvertStringToByteArrayUtf8(string str);
        byte[] ConvertStringToByteArrayAscii(string str);
        byte[] RemoveTraillingNullFromByteArray(byte[] bytes);
    }
}
