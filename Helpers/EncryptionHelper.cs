using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Helpers
{
    public class EncryptionHelper : IEncryptionHelper
    {
        private static readonly Lazy<IEncryptionHelper> _instance = new Lazy<IEncryptionHelper>(() => new EncryptionHelper());

        public static IEncryptionHelper Instance
        {
            get
            {
                return _instance.Value;
            }
        }
        public byte[] EncryptString_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                aesAlg.Padding = PaddingMode.Zeros;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        public string DecryptString_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                aesAlg.Padding = PaddingMode.Zeros;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext.Replace("\0", "");
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
