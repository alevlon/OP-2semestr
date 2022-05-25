using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace KPI_DELIVERY
{
    public class Hash
    {
        private string password { get; set; }
        private string encryptedPassword { get; set; }
        internal Hash(string a)
        {
            this.password = a;
        }

        public string Encrypt() 
        {
            SHA256 sha256Hash = SHA256.Create();
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            encryptedPassword = builder.ToString();

            return encryptedPassword;
        }
    }
}
