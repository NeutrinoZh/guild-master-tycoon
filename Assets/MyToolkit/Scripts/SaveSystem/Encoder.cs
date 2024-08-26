using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MTK.SaveSystem
{
    internal class Encoder
    {
        public static string Encode(string value, string key)
        {
            byte[] initVector = new byte[16];
            byte[] dataArray;

            using Aes aes = Aes.Create();
            using SHA256 sha256 = SHA256.Create();

            aes.Key = sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
            aes.IV = initVector;

            ICryptoTransform encoder = aes.CreateEncryptor(aes.Key, aes.IV);

            using MemoryStream memStream = new();
            using CryptoStream cryptoStream = new(memStream, encoder, CryptoStreamMode.Write);
            using StreamWriter writer = new(cryptoStream);

            writer.Write(value);
            dataArray = memStream.ToArray();

            return Convert.ToBase64String(dataArray);
        }

        public static string Decode(string value, string key)
        {
            byte[] initVector = new byte[16];
            byte[] buffer = Convert.FromBase64String(value);

            using Aes aes = Aes.Create();
            using SHA256 sha256 = SHA256.Create();

            aes.Key = sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
            aes.IV = initVector;

            ICryptoTransform decoder = aes.CreateDecryptor(aes.Key, aes.IV);

            using MemoryStream memoryStream = new(buffer);
            using CryptoStream cryptoStream = new(memoryStream, decoder, CryptoStreamMode.Read);
            using StreamReader streamReader = new(cryptoStream);

            return streamReader.ReadToEnd();
        }
    }
}