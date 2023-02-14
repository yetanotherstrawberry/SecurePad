using SecurePad.Interfaces;
using SecurePad.Models;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Xamarin.Forms;

namespace SecurePad.Helpers
{
    internal static class AesClass
    {

        public static byte[] CreateHashedKey(byte[] password)
        {
            var salt = Encoding.Default.GetBytes(DependencyService.Get<ISalt>().GetSalt());
            const int iterations = 300;
            Rfc2898DeriveBytes keyGenerator = new Rfc2898DeriveBytes(password, salt, iterations);
            return keyGenerator.GetBytes(password.Length);
        }

        public static CryptoInfo Encrypt(string clearValue, byte[] encryptionKey)
        {
            using Aes aes = Aes.Create();
            aes.Key = CreateHashedKey(encryptionKey);

            byte[] encrypted = AesEncryptStringToBytes(clearValue, aes.Key, aes.IV);
            return new CryptoInfo { InitVec = aes.IV, CryptoMsg = encrypted };
        }

        private static byte[] AesEncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException(nameof(plainText));
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException(nameof(key));
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException(nameof(iv));

            byte[] encrypted;

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using MemoryStream memoryStream = new MemoryStream();
                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                {
                    streamWriter.Write(plainText);
                }
                encrypted = memoryStream.ToArray();
            }
            return encrypted;
        }

        public static string Decrypt(string encryptedValue, byte[] encryptionKey)
        {
            var cryptoData = JsonSerializer.Deserialize<CryptoInfo>(encryptedValue);
            return AesDecryptStringFromBytes(cryptoData.CryptoMsg, CreateHashedKey(encryptionKey), cryptoData.InitVec);
        }

        private static string AesDecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            if (cipherText == null || cipherText.Length < 1)
                throw new ArgumentNullException(nameof(cipherText));
            if (key == null || key.Length < 1)
                throw new ArgumentNullException(nameof(key));
            if (iv == null || iv.Length < 1)
                throw new ArgumentNullException(nameof(iv));

            var plaintext = string.Empty;

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using MemoryStream memoryStream = new MemoryStream(cipherText);
                using ICryptoTransform decryptor = aes.CreateDecryptor();
                using CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                using StreamReader streamReader = new StreamReader(cryptoStream);
                plaintext = streamReader.ReadToEnd();
            }

            return plaintext;
        }

    }
}
