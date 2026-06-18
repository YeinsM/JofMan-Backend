using System.Security.Cryptography;
using System.Text;

namespace BMS_Logistics.Infrastructure.Helpers
{
    public class PasswordHelper
    {
        public static string Encrypt(string pass)
        {
            // Encryption Type
            // Converts the received “input” parameter into a byte array and calculates the hash.
            byte[] data = MD5.HashData(Encoding.UTF8.GetBytes(pass));
            StringBuilder sBuilder = new();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}
