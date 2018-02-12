using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Shared
{
    public class PasswordFactory
    {

        private const int saltBytes = 128 / 8;
        private const int keyBytes = 256 / 8;

        /// <summary>
        /// Compares a plain(unhashed) to a hashed password
        /// </summary>
        /// <param name="plain">Unhashed password</param>
        /// <param name="hashed">Password hashed using HassPassword</param>
        /// <returns>true if plain and hashes are the same hash</returns>
        public static bool ComparePasswords(string plain, string hashed)
        {
            if (hashed.Length != 68)
                throw new ArgumentException("Hashed password length must be 68");

            byte[] salt = Convert.FromBase64String(hashed.Substring(0,24));
            if (salt.Length != saltBytes)
                throw new ArgumentException("Salt length must be " + saltBytes + " bytes");

            return HashPassword(plain, salt) == hashed;
        }

        private static string HashPassword(string password, byte[] salt)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: keyBytes));
            return Convert.ToBase64String(salt) + hashed;
        }

        /// <summary>
        /// Safely hashes a string
        /// </summary>
        /// <param name="password"></param>
        /// <returns>Hashes salt and password, 68 chars long</returns>
        public static string HashPassword(string password)
        {
            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[saltBytes];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return HashPassword(password, salt);
        }
    }
}
