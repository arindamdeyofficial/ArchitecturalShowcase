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

        //peoblem with IDataProtector is key will be max for 90 days
        //private readonly IDataProtector _protector;  
        private static readonly Aes _myAes;
        private static readonly byte[] _key;
        private static readonly byte[] _four;
        static EncryptionHelper()
        {
            //_protector = provider.CreateProtector(GetType().FullName);
            _myAes = Aes.Create();
            _key = _myAes.Key;
            _four = _myAes.IV;
        }
        public static IEncryptionHelper Instance
        {
            get
            {
                return _instance.Value;
            }
        }
        public EncryptionDetails EncryptString_Aes(string plainText)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = _key;
                aesAlg.IV = _four;
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
            return new EncryptionDetails
            {
                EncryptedText = Convert.ToBase64String(encrypted),
                Key = Convert.ToBase64String(_key),
                Iv = Convert.ToBase64String(_four),
                PlainText = plainText
            };
        }

        public EncryptionDetails DecryptString_Aes(EncryptionDetails cipherDetails)
        {
            // Check arguments.
            if (cipherDetails == null || cipherDetails?.EncryptedText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            string cipherText = cipherDetails?.EncryptedText;

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Convert.FromBase64String(cipherDetails.Key);
                aesAlg.IV = Convert.FromBase64String(cipherDetails.Iv);
                aesAlg.Padding = PaddingMode.Zeros;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
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

            return new EncryptionDetails
            {
                EncryptedText = plaintext.Replace("\0", ""),
                Key = Convert.ToBase64String(_key),
                Iv = Convert.ToBase64String(_four),
                PlainText = cipherText
            };
        }
    }
    public class EncryptionDetails
    {
        public string EncryptedText { get; set; }
        public string PlainText { get; set; }
        public string Key { get; set; }
        public string Iv { get; set; }
    }
}
