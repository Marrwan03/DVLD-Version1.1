using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness.Cryptography_Classes
{
    public class clsAsymmetric
    {
        // How to generate Public Key and Private Key...

               //using(RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
               // {
                     //string PublicKey = rsa.ToXmlString(false);
                     //string PrivateKey = rsa.ToXmlString(true);
               // } 



    public static string Encrypt(string PlainText, string PublicKey)
        {
            try
            {
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.FromXmlString(PublicKey);

                    byte[] ecryptedData = RSA.Encrypt(Encoding.UTF8.GetBytes(PlainText), false);

                    return Convert.ToBase64String(ecryptedData);
                }
            }
            catch (Exception ex)
            {
               // WriteLine(ex.ToString());
                throw;
            }
        }

        public static string Decrypt(string cipherText, string PrivateKey)
        {
            try
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(PrivateKey);

                    byte[] encryptData = Convert.FromBase64String(cipherText);
                    byte[] decryptData = rsa.Decrypt(encryptData, false);

                    return Encoding.UTF8.GetString(decryptData);
                }
            }
            catch (Exception ex)
            {
               // WriteLine($"{ex.Message}");
                throw;
            }
        }

    }
}
