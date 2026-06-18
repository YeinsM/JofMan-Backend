using BMS_Logistics.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using Aes = System.Security.Cryptography.Aes;

namespace BMS_Logistics.Infrastructure.Helpers
{
    public class DecryptHelper : IDecryptHelper
    {
        private readonly IConfiguration _configuration;

        public DecryptHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Encrypt(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {

                aesAlg.Key = Encoding.UTF8.GetBytes(_configuration["PrivateKey:BackendKey"]!);
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using MemoryStream msEncrypt = new();
                using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
                using (StreamWriter swEncrypt = new(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }
                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }

        public string DecryptBackend(string cipherText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(_configuration["PrivateKey:BackendKey"]!);
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                byte[] decryptedBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }

        public string Decrypt(string data, bool? backend = false)
        {
            byte[] ciphering;
            string cipher = string.Empty;

            if (backend == false)
            {
                // Convert the cipher to an array of bytes
                ciphering = Convert.FromBase64String(data);
                // Convert the bytes to a string
                cipher = Encoding.UTF8.GetString(ciphering);
            }

            // Create a new instance of the MD5CryptoServiceProvider class
            using MD5CryptoServiceProvider md5 = new();

            // Create a new instance of the TripleDESCryptoServiceProvider class
            using TripleDESCryptoServiceProvider tdes = new();

            // Set the Key property of the TripleDESCryptoServiceProvider to the MD5 hash value
            tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(_configuration["PrivateKey:Key"]!));
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            // Create a new instance of the ICryptoTransform interface
            ICryptoTransform transform = tdes.CreateDecryptor();

            // Convert the cipher to an array of bytes
            byte[] cipherBytes = Convert.FromBase64String(backend != false ? data : cipher);
            byte[] bytes = transform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
            return UTF8Encoding.UTF8.GetString(bytes);
        }
    }
}
