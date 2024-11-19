using System.Security.Cryptography;
using System.Text;

namespace SmartMatrix.Core.DataSecurity
{
    public class CryptographyTool
    {
        public static string GetMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (MD5 algo = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = algo.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString().ToUpper();
            }
        }

        public static string GetSHA512(string input)
        {
            // Use input string to calculate MD5 hash
            using (SHA512 algo = new SHA512Managed())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = algo.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString().ToUpper();
            }
        }
    }
}