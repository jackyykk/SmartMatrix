using System;
using System.Security.Cryptography;

namespace SmartMatrix.Core.DataSecurity
{
    public class MyHashTool
    {
        private const int SaltSize = 16; // 128 bit
        private const int KeySize = 32; // 256 bit
        private const int Iterations = 10000;

        public static string ComputePasswordHash(string password, out string salt)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var saltBytes = new byte[SaltSize];
                rng.GetBytes(saltBytes);
                salt = Convert.ToBase64String(saltBytes);

                using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, Iterations))
                {
                    var hash = deriveBytes.GetBytes(KeySize);
                    return Convert.ToBase64String(hash);
                }
            }
        }

        public static bool VerifyPasswordHash(string password, string salt, string hash)
        {
            var saltBytes = Convert.FromBase64String(salt);
            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, Iterations))
            {
                var hashBytes = deriveBytes.GetBytes(KeySize);
                var hashToCompare = Convert.ToBase64String(hashBytes);
                return hash == hashToCompare;
            }
        }
    }
}