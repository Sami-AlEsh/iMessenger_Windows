using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace iMessenger.Scripts.AES
{
    public class AESOperation
    {
        /// <summary>
        /// Generates AES-128 bits Key
        /// </summary>
        /// <returns></returns>
        public static string GenerateKey()
        {
            RijndaelManaged aesEncryption = new RijndaelManaged();
            aesEncryption.KeySize = 128; //in bits
            aesEncryption.BlockSize = 128;
            aesEncryption.Mode = CipherMode.CBC;
            aesEncryption.Padding = PaddingMode.PKCS7;
            aesEncryption.GenerateIV();
            aesEncryption.GenerateKey();

            return Convert.ToBase64String(aesEncryption.Key);
        }

        /// <summary>
        /// Encrypt Text string using AES-128 Key
        /// </summary>
        /// <param name="key">AES-128 Key</param>
        /// <param name="plainText">Text to Encrypt</param>
        /// <returns>Encrypted Text</returns>
        public static string Encrypt(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                //aes.Key = Encoding.UTF8.GetBytes(key);
                aes.Key = Convert.FromBase64String(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        /// <summary>
        /// Decrypt Text string using AES-128 Key
        /// </summary>
        /// <param name="key">AES-128 Key</param>
        /// <param name="cipherText">Encrypted Text to Decrypt</param>
        /// <returns>Plain Text</returns>
        public static string Decrypt(string key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                //aes.Key = Encoding.UTF8.GetBytes(key);
                aes.Key = Convert.FromBase64String(key);
                aes.IV = iv;
                aes.Padding = PaddingMode.PKCS7;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Encrypt File(Byte Array) using AES-128 Key
        /// </summary>
        /// <param name="Key">AES-128 Key</param>
        /// <param name="plainData">Byte Array to Encrypt</param>
        /// <returns>Encrypted Data(Byte Array)</returns>
        public static byte[] Encrypt(string Key, byte[] plainData)
        {
            var key = Convert.FromBase64String(Key);
            var iv = new byte[16];
            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.KeySize = 128;
                aesAlg.BlockSize = 128;
                aesAlg.FeedbackSize = 128;
                aesAlg.Padding = PaddingMode.PKCS7;
                aesAlg.Key = key;
                aesAlg.IV = iv;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(plainData, 0, plainData.Length);
                        csEncrypt.FlushFinalBlock();

                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }
        /// <summary>
        /// Decrypt File(Byte Array) using AES-128 Key
        /// </summary>
        /// <param name="Key">AES-128 Key</param>
        /// <param name="cipherData">Encrypted Data(Byte Array)</param>
        /// <returns>Decrypted Data(Plain Byte Array)</returns> 
        public static byte[] Decrypt(string Key, byte[] cipherData)
        {
            var iv = new byte[16];
            using (var aes = Aes.Create())
            {
                aes.KeySize = 128;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.PKCS7;

                aes.Key = Convert.FromBase64String(Key);
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    return PerformCryptography(cipherData, decryptor);
                }
            }
        }
        private static byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
        {
            using (var ms = new MemoryStream())
            using (var cryptoStream = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
            {
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();

                return ms.ToArray();
            }
        }

        /// <summary>
        /// Stores the given AES-Keys securely in local storage
        /// </summary>
        /// <param name="AESkeys"></param>
        public static void StoreKeys(Dictionary<string, string> AESkeys)
        {
            var JKeys = JsonConvert.SerializeObject(AESkeys);
            var P1 = Protector.Protect(Encoding.Unicode.GetBytes(JKeys));
            File.WriteAllBytes(Project.Path + "/Keys/AESSecret.keys", P1);
        }

        /// <summary>
        /// Retrieve secure AES-Keys from local storage
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetKey()
        {
            var P1 = Protector.Unprotect(File.ReadAllBytes(Project.Path + "/Keys/AESSecret.keys"));
            var JKeys = Encoding.Unicode.GetString(P1);
            Dictionary<string, string> AesKeys = JsonConvert.DeserializeObject<Dictionary<string, string>>(JKeys);
            return AesKeys;
        }
    }
}