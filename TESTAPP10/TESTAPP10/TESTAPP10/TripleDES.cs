using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TESTAPP10
{
    public static class TripleDES
    {
        private static byte[] key = new byte[] { 3, 19, 1, 19, 15, 6, 20, 23, 1, 18, 5, 9, 14, 3, 4, 1, 22, 5, 6, 9, 19, 8, 5, 18 };

        public static string Encrypt(string plainText)
        {
            // Declare a UTF8Encoding object so we may use the GetByte 
            // method to transform the plainText into a Byte array. 
            UTF8Encoding utf8encoder = new UTF8Encoding();
            byte[] inputInBytes = utf8encoder.GetBytes(plainText);
            RNGCryptoServiceProvider RNG = new RNGCryptoServiceProvider();

            byte[] iv = new byte[8];

            RNG.GetBytes(iv);

            // Create a new TripleDES service provider 
            TripleDESCryptoServiceProvider tdesProvider = new TripleDESCryptoServiceProvider();

            // The ICryptTransform interface uses the TripleDES 
            // crypt provider along with encryption key and init vector 
            // information 
            ICryptoTransform cryptoTransform = tdesProvider.CreateEncryptor(key, iv);

            // All cryptographic functions need a stream to output the 
            // encrypted information. Here we declare a memory stream 
            // for this purpose. 
            MemoryStream encryptedStream = new MemoryStream();
            CryptoStream cryptStream = new CryptoStream(encryptedStream, cryptoTransform, CryptoStreamMode.Write);

            // Write the encrypted information to the stream. Flush the information 
            // when done to ensure everything is out of the buffer. 
            cryptStream.Write(inputInBytes, 0, inputInBytes.Length);
            cryptStream.FlushFinalBlock();
            encryptedStream.Position = 0;

            // Read the stream back into a Byte array and return it to the calling 
            // method. 
            byte[] result = new byte[encryptedStream.Length - 1 + 1];
            encryptedStream.Read(result, 0, Convert.ToInt32(encryptedStream.Length));
            cryptStream.Close();

            byte[] Data = new byte[iv.Length + result.Length - 1 + 1];
            Buffer.BlockCopy(iv, 0, Data, 0, iv.Length);
            Buffer.BlockCopy(result, 0, Data, iv.Length, result.Length);

            return Convert.ToBase64String(Data, 0, Data.Length);
        }

    }
}
