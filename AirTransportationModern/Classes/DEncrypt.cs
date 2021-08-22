using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AirTransportationModern.Classes
{
    public static class DEncrypt
    {
        private const string Key = "****";

        public static string Encrypt(string Value)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(Value);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(Key);

            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            ICryptoTransform transform = provider.CreateEncryptor(passwordBytes, passwordBytes);

            MemoryStream MS = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(MS, transform, CryptoStreamMode.Write);

            cryptoStream.Write(messageBytes, 0, messageBytes.Length);
            cryptoStream.FlushFinalBlock();

            byte[] encryptedMessageBytes = new byte[MS.Length];
            MS.Position = 0;
            MS.Read(encryptedMessageBytes, 0, encryptedMessageBytes.Length);

            string encryptedMessage = Convert.ToBase64String(encryptedMessageBytes);

            return encryptedMessage;
        }
        public static string Decrypt(string encryptedMessage)
        {
            byte[] encryptedMessageBytes = Convert.FromBase64String(encryptedMessage);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(Key);

            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            ICryptoTransform transform = provider.CreateDecryptor(passwordBytes, passwordBytes);
            CryptoStreamMode mode = CryptoStreamMode.Write;

            MemoryStream memStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memStream, transform, mode);
            cryptoStream.Write(encryptedMessageBytes, 0, encryptedMessageBytes.Length);
            cryptoStream.FlushFinalBlock();

            byte[] decryptedMessageBytes = new byte[memStream.Length];
            memStream.Position = 0;
            memStream.Read(decryptedMessageBytes, 0, decryptedMessageBytes.Length);

            string message = Encoding.UTF8.GetString(decryptedMessageBytes);

            return message;
        }
    }
}
