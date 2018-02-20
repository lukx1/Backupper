﻿using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

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

            return HashPasswordPbkdf2(plain, salt) == hashed;
        }

        /// <summary>
        /// Vypočítá CRC32 stringu, pouze pro kontrolu.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static uint CalculateCRC32(string input)
        {
            Crc32 crc = new Crc32();
            return crc.ComputeChecksum(Encoding.UTF8.GetBytes(input));
        }

        private static string HashPasswordPbkdf2(string password, byte[] salt)
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

        //private static readonly byte[] SALT = new byte[] { 0x26, 0xdc, 0xff, 0x00, 0xad, 0xed, 0x7a, 0xee, 0xc5, 0xfe, 0x07, 0xaf, 0x4d, 0x08, 0x22, 0x3c };
        //private static readonly byte[] IV = new byte[]   { 1,7,255,0,173,237,122,238,2,254,7,5,77,8,34,60 };

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
            cryptoStream.Write(Encoding.UTF8.GetBytes(message), 0, message.Length);
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
            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }
    }
}