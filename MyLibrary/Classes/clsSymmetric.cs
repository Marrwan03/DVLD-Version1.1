using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness.Classes
{
    public class clsSymmetric
    {
        public static string Encrypt(string CardNumber, string Key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(Key);
                aesAlg.IV = new byte[aesAlg.BlockSize / 8];

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var mnEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(mnEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(CardNumber);
                        }
                    }
                    return Convert.ToBase64String(mnEncrypt.ToArray());
                }

            }



        }

        public static string Decrypt(string CardNumber, string Key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(Key);
                aesAlg.IV = new byte[aesAlg.BlockSize / 8];

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var mnDecrypt = new MemoryStream(Convert.FromBase64String(CardNumber)))
                {
                    using (var csDecrypt = new CryptoStream(mnDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var stDecrypt = new StreamReader(csDecrypt))
                        {
                            return stDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
        static void GenerateKeyValue(string Path, string Key)
        {
            using (StreamWriter writer = new StreamWriter(Path))
            {
                writer.WriteLine(ReverseValue(Key));
            }
        }

        public static string ReverseValue(string Value)
        {
            string KeyValue = "";
            var V = Value.Reverse();
            foreach (var item in V)
            {
                KeyValue += item;
            }
            return KeyValue;
        }

        public static string GetKeyValue()
        {
            string Value;
            string Path = Directory.GetCurrentDirectory() + @"\KeyValue.txt";
            if (!File.Exists(Path))
            {
                GenerateKeyValue(Path, "1h2ff5e4rw99ju2i");
            }
            using (StreamReader reader = new StreamReader(Path))
            {
                Value = ReverseValue(reader.ReadLine());

            }
            return Value;
        }
    }
}
