using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Helpers
{
    public class ConversionHelper : IConversionHelper
    {
        private static readonly Lazy<IConversionHelper> _instance = new Lazy<IConversionHelper>(() => new ConversionHelper());

        public static IConversionHelper Instance
        {
            get
            {
                return _instance.Value;
            }
        }
        public byte[] ConvertStringToByteArrayAscii(string str)
        {
            Byte[] bytes;
            int byteCount = Encoding.ASCII.GetByteCount(str.ToCharArray(), 0, str.Length);
            bytes = new Byte[byteCount];
            var converted = Encoding.ASCII.GetBytes(str, 0, str.Length, bytes, 0);
            return (converted > 0) ? bytes : new byte[0];
        }
        public byte[] ConvertStringToByteArrayUtf8(string str)
        {
            Byte[] bytes;
            int byteCount = Encoding.UTF8.GetByteCount(str.ToCharArray(), 0, str.Length);
            bytes = new Byte[byteCount*2];
            var converted = Encoding.UTF8.GetBytes(str, 0, str.Length, bytes, 0);
            return (converted > 0) ? RemoveTraillingNullFromByteArray(bytes) : new byte[0];
        }
        public string ConvertByteArrayToStringAscii(byte[] str)
        {            
            return Encoding.ASCII.GetString(str);
        }
        public string ConvertByteArrayToStringUtf8(byte[] str)
        {
            return Encoding.UTF8.GetString(str);
        }
        public byte[] RemoveTraillingNullFromByteArray(byte[] bytes)
        {
            return bytes.TakeWhile((v, index) => bytes.Skip(index).Any(w => w != 0x00)).ToArray();
        }
    }
}
