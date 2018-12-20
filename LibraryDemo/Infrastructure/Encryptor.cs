using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDemo.Infrastructure
{
    public class Encryptor
    {
        private Encryptor()
        {
        }

        public static string MD5Encrypt32(string password)
        {
            MD5 md5 = MD5.Create();
            byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder hashPassword = new StringBuilder();
            foreach (var b in hashBytes)
            {
                hashPassword.Append(b);
            }

            return hashPassword.ToString();
        }
    }
}
