using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace rmt.Classes
{
    public class Crypto
    {
        public static string HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                               password: password,
                                              salt: salt,
                                                             prf: KeyDerivationPrf.HMACSHA1,
                                                                            iterationCount: 10000,
                                                                                           numBytesRequested: 256 / 8));

            return $"{Convert.ToBase64String(salt)}:{hashed}";
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            string[] parts = hashedPassword.Split(':');
            byte[] salt = Convert.FromBase64String(parts[0]);
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                               password: password,
                                              salt: salt,
                                                             prf: KeyDerivationPrf.HMACSHA1,
                                                                            iterationCount: 10000,
                                                                                           numBytesRequested: 256 / 8));

            return parts[1] == hashed;
        }
    }   
}