using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace Shared
{
    /// <summary>
    /// Tvoří, šifruje a porovnává hesla. 
    /// </summary>
    public class PasswordFactory
    {

        private const int saltBytes = 128 / 8;
        private const int keyBytes = 256 / 8;

        /// <summary>
        /// Porovnává a obyčejné(nešifrované) heslo s šifrovaným heslem
        /// </summary>
        /// <param name="plain">Nešifrované heslo</param>
        /// <param name="hashed">Heslo šifrované pomocí HashPasswordPbkdf2</param>
        /// <returns>Pravda pokud hesla jsou shodná</returns>
        public static bool ComparePasswordsPbkdf2(string plain, string hashed)
        {
            if (hashed.Length != 68)
                throw new ArgumentException("Hashed password length must be 68");

            byte[] salt = Convert.FromBase64String(hashed.Substring(0, 24));
            if (salt.Length != saltBytes)
                throw new ArgumentException("Salt length must be " + saltBytes + " bytes");

            return SlowEquals(Encoding.ASCII.GetBytes(HashPasswordPbkdf2(plain, salt)),Encoding.ASCII.GetBytes(hashed));
        }

        /// <summary>
        /// Vypočítá CRC32 stringu, pouze pro kontrolu.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static uint CalculateCRC32(string input)
        {
            Crc32 crc = new Crc32();
            return crc.ComputeChecksum(Encoding.ASCII.GetBytes(input));
        }

        private static bool SlowEquals(byte[] a, byte[] b)
        {
            var diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }

        private static string HashPasswordPbkdf2(string password, byte[] salt)
        {
            /*string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: keyBytes));*/
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(password, salt);
            rfc.IterationCount = 10000;
            return Convert.ToBase64String(salt) + Convert.ToBase64String(rfc.GetBytes(keyBytes));
        }

        /// <summary>
        /// Bezpečně asymetricky zašifruje string
        /// </summary>
        /// <param name="password"></param>
        /// <returns>Šifrované heslo se solí, 68 char dlouhé</returns>
        public static string HashPasswordPbkdf2(string password)
        {
            return HashPasswordPbkdf2(password, GenerateRandomBytes(128/8));
        }

        /// <summary>
        /// Vytvoří bezpečně náhodné heslo o žádané délce
        /// </summary>
        /// <param name="bytes">Z kolika bytů bude heslo vytvořeno</param>
        /// <returns></returns>
        public static string CreateRandomPassword(int bytes)
        {
            return Convert.ToBase64String(GenerateRandomBytes(bytes));
        }

        private static byte[] GenerateRandomBytes(int count)
        {
            byte[] b = new byte[saltBytes];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(b);
            }
            return b;
        }

        public byte[] HashSha1(byte[] bytes)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                return sha1.ComputeHash(bytes);
            }
        }

        /// <summary>
        /// Symetricky zašifruje zprávu pomocí hesla
        /// </summary>
        /// <param name="message"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string EncryptAES(string message, string password)
        {
            byte[] iv = GenerateRandomBytes(16);
            byte[] salt = GenerateRandomBytes(16);
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            MemoryStream memoryStream;
            CryptoStream cryptoStream;
            Rijndael rijndael = Rijndael.Create();
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, salt);
            rijndael.Key = pdb.GetBytes(32);
            rijndael.IV = iv;
            memoryStream = new MemoryStream();
            cryptoStream = new CryptoStream(memoryStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
            cryptoStream.Close();
            return Convert.ToBase64String(salt) + Convert.ToBase64String(iv) + Convert.ToBase64String(memoryStream.ToArray());
        }

        /// <summary>
        /// Dešifruje zprávu vytvořenou EncryptAES pomocí hesla
        /// </summary>
        /// <param name="cipher"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string DecryptAES(string cipher, string password)
        {
            MemoryStream memoryStream;
            CryptoStream cryptoStream;
            Rijndael rijndael = Rijndael.Create();
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, Convert.FromBase64String(cipher.Substring(0, 24)));
            //byte[] cipherBytes = Convert.FromBase64String(cipher);
            rijndael.Key = pdb.GetBytes(32);
            rijndael.IV = Convert.FromBase64String(cipher.Substring(24, 24));
            memoryStream = new MemoryStream();
            cryptoStream = new CryptoStream(memoryStream, rijndael.CreateDecryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(Convert.FromBase64String(cipher.Substring(48)), 0, Convert.FromBase64String(cipher.Substring(48)).Length);
            cryptoStream.Close();
            return Encoding.ASCII.GetString(memoryStream.ToArray());
        }
    }
}