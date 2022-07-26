using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace act2_8
{
    class Encrypt
    {
        public static byte[] GenerateSalt()
        {
            const int saltLenght = 32;
            using (var randomNymberGenerator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[saltLenght];
                randomNymberGenerator.GetBytes(randomNumber);
                return randomNumber;
            }
        }

        private static byte[] Combine(byte[] first, byte[] second)
        {

            //Que copiar, de donde, a donde, 
            var ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length,second.Length);

            return ret;
        }

        public static byte[] HashPasswordWithSalt(byte[] toBeHashed, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var combinedHash = Combine(toBeHashed, salt);
                return sha256.ComputeHash(combinedHash);
            }
        }



    }
}
