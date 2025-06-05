using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace LocalCode.Common
{
    public static class CryptoUtil
    {
        private static readonly string key = "1234567890123456"; // 16位密钥
        private static readonly string iv = "1234567890123456";  // 16位IV

        public static string Encrypt(string plainText)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(iv);
                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                var plainBytes = Encoding.UTF8.GetBytes(plainText);
                var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                return System.Convert.ToBase64String(cipherBytes);
            }
        }

        public static string Decrypt(string cipherText)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(iv);
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                var cipherBytes = System.Convert.FromBase64String(cipherText);
                var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                return Encoding.UTF8.GetString(plainBytes);
            }
        }
        public static void ExampleUsage(string json)
        {
            var encrypted = CryptoUtil.Encrypt(json);
            var decrypted = CryptoUtil.Decrypt(encrypted);
        }
    }
}