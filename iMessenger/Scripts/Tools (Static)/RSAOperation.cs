using System;
using System.Security.Cryptography;

namespace iMessenger.Scripts.RSA
{
    class RSAOperation
    {
        /// <summary>
        /// Encrypt Byte Array using RSA public Key
        /// </summary>
        /// <param name="plainData">Data to Encrypt</param>
        /// <param name="pubKey">RSA Public Key</param>
        /// <returns>Encrypted Byte Array</returns>
        static public byte[] Encryption(byte[] plainData, RSAParameters pubKey)
        {
            try
            {
                var RSA = new RSACryptoServiceProvider();
                
                //Get Public Key
                RSA.ImportParameters(pubKey);
                //Encrypt Data
                var bytesCypher = RSA.Encrypt(plainData, false);

                return bytesCypher;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("RSA Encryption Error" + e.Message);
                return null;
            }
        }

        /// <summary>
        /// Decrypt Byte Array using RSA private Key
        /// </summary>
        /// <param name="cypherData">Encrypted Data to Decrypt</param>
        /// <param name="privKey">RSA Private Key</param>
        /// <returns>Plain Data(Decrypted) as Byte Array</returns>
        static public byte[] Decryption(byte[] cypherData, RSAParameters privKey)
        {
            try
            {
                var RSA = new RSACryptoServiceProvider();

                //Get Private Key
                RSA.ImportParameters(privKey);
                //Decrypt Data
                var plainData = RSA.Decrypt(cypherData, false);

                return plainData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("RSA Decryption Error " + e.ToString());
                return null;
            }
        }
    }


    [Serializable]
    public class RSA_keys
    {
        public RSAParameters publicKey { get; set; }
        public RSAParameters privateKey { get; set; }

        /// <summary>
        /// Generate new RSA Keys
        /// </summary>
        public RSA_keys()
        {
            var RSA = new RSACryptoServiceProvider(2048);
            this.privateKey = RSA.ExportParameters(true);
            this.publicKey  = RSA.ExportParameters(false);
        }
    }
}

