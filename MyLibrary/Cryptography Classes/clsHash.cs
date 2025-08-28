using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness.Global_Classes
{
    public class clsHash
    {
        public static bool IsPasswordCorrect(string Password, byte[] storeHashing, byte[] storeSalt)
        {
            var Hash = ComputeHashWithSalt(Password, storeSalt);
            return Hash.SequenceEqual(storeHashing);

        }

        public static byte[] GenerateSalt(int SaltSize = 16)
        {

            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] Salt = new byte[SaltSize];
                rng.GetBytes(Salt);
                return Salt;
            }

        }

        public static byte[] ComputeHashWithSalt(string Password, byte[] Salt)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                var PasswordByte = Encoding.UTF8.GetBytes(Password); ;
                byte[] PasswordWithSalt = new byte[PasswordByte.Length + Salt.Length];

                Buffer.BlockCopy(Salt, 0, PasswordWithSalt, 0, Salt.Length);
                Buffer.BlockCopy(PasswordByte, 0, PasswordWithSalt, Salt.Length, PasswordByte.Length);

                return sha256.ComputeHash(PasswordWithSalt);
            }

        }
    }
}
