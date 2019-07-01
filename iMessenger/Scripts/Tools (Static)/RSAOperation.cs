using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

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


        /// <summary>
		/// Verifies the signature for a given data.
		/// </summary>
		/// <param name="signature">The signature </param>
		/// <param name="signedData">Original data in Base64</param>
		/// <returns></returns>
		private bool verifySignature(RSAParameters PublicKey, string sign, string signedData)
        {
            byte[] signature = Convert.FromBase64String(sign);
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSA.ImportParameters(PublicKey);

            byte[] hash = Convert.FromBase64String(signedData);
            try
            {
                if (RSA.VerifyData(hash, "SHA1", signature))
                {
                    //Console.WriteLine("The signature is valid.");
                    return true;
                }
                else
                {
                    //Console.WriteLine("The signature is not valid.");
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }


    [Serializable]
    public class RSA_keys
    {
        public RSAParameters publicKey { get; set; }
        public RSAParameters privateKey { get; set; }

        /// <summary>
        /// Generate new RSA 2048 Keys
        /// </summary>
        public RSA_keys()
        {
            var RSA = new RSACryptoServiceProvider(2048);
            this.privateKey = RSA.ExportParameters(true);
            this.publicKey  = RSA.ExportParameters(false);
        }

        /// <summary>
        /// Returns Public Key from Java XML PublicKey
        /// </summary>
        /// <param name="JavaXMLPublicKey"></param>
        /// <returns></returns>
        public static RSAParameters FromJavaXML(string JavaXMLPublicKey)
        {
            RSAParameters RSAPublicKey = new RSAParameters();
            string modStr = "";
            string expStr = "";
            // read the XML formated public key 
            try
            {

                XmlTextReader reader = new XmlTextReader(new StringReader(JavaXMLPublicKey));
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == "Modulus")
                        {
                            reader.Read();
                            modStr = reader.Value;
                        }
                        else if (reader.Name == "Exponent")
                        {
                            reader.Read();
                            expStr = reader.Value;
                        }
                    }
                }
                if (modStr.Equals("") || expStr.Equals(""))
                {
                    //throw exception
                    throw new Exception("Invalid public key");
                }
                RSAPublicKey.Modulus = Convert.FromBase64String(modStr);
                RSAPublicKey.Exponent = Convert.FromBase64String(expStr);
                return RSAPublicKey;
            }
            catch (Exception)
            {
                throw new Exception("Invalid Public Key.");
            }
        }

        public static void StoreKeys(RSA_keys keys)
        {
            RSACryptoServiceProvider RSA1 = new RSACryptoServiceProvider();
            RSA1.ImportParameters(keys.privateKey);
            var P1 = Protector.Protect(Encoding.Unicode.GetBytes(RSA1.ToXmlString(true)));
            File.WriteAllBytes(Project.Path + "/Keys/RSAPrivate.key", P1);

            RSACryptoServiceProvider RSA2 = new RSACryptoServiceProvider();
            RSA2.ImportParameters(keys.publicKey);
            var P2 = Protector.Protect(Encoding.Unicode.GetBytes(RSA2.ToXmlString(false)));
            File.WriteAllBytes(Project.Path + "/Keys/RSAPublic.key", P2);
        }

        public static RSA_keys GetKeys()
        {
            RSA_keys keys = new RSA_keys();

            var P1 = Protector.Unprotect(File.ReadAllBytes(Project.Path + "/Keys/RSAPrivate.key"));
            RSACryptoServiceProvider RSA1 = new RSACryptoServiceProvider();
            RSA1.FromXmlString(Encoding.Unicode.GetString(P1));
            keys.privateKey = RSA1.ExportParameters(true);

            var P2 = Protector.Unprotect(File.ReadAllBytes(Project.Path + "/Keys/RSAPublic.key"));
            RSACryptoServiceProvider RSA2 = new RSACryptoServiceProvider();
            RSA2.FromXmlString(Encoding.Unicode.GetString(P2));
            keys.publicKey = RSA2.ExportParameters(false);

            return keys;
        }
    }
}