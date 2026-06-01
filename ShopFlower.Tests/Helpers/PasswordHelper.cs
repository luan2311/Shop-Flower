using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ShopFlower.Tests.Helpers
{
    public static class PasswordHelper
    {
        public const int SaltSize = 16;
        public const int HashSize = 32;
        public const int Iterations = 10000;

        public static (byte[] hash, byte[] salt) HashPasswordPBKDF2(string password)
        {
            var salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                var hash = pbkdf2.GetBytes(HashSize);
                return (hash, salt);
            }
        }

        public static bool VerifyPasswordPBKDF2(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, storedSalt, Iterations))
            {
                var computed = pbkdf2.GetBytes(HashSize);
                return computed.SequenceEqual(storedHash);
            }
        }

        public static (byte[] hash, byte[] salt) HashPasswordSHA256Legacy(string password)
        {
            // DĂ¹ng Guid.NewGuid().ToByteArray() nhÆ° AccountController.Dang_ky()
            var salt = Guid.NewGuid().ToByteArray(); // 16 bytes
            var pwdBytes = Encoding.UTF8.GetBytes(password);
            var concat = new byte[pwdBytes.Length + salt.Length];
            Buffer.BlockCopy(pwdBytes, 0, concat, 0, pwdBytes.Length);
            Buffer.BlockCopy(salt, 0, concat, pwdBytes.Length, salt.Length);

            byte[] hash;
            using (var sha = SHA256.Create())
            {
                hash = sha.ComputeHash(concat);
            }
            return (hash, salt);
        }

        public static bool VerifyPasswordSHA256Legacy(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var sha = SHA256.Create())
            {
                var pwdBytes = Encoding.UTF8.GetBytes(password);
                var concat = new byte[pwdBytes.Length + storedSalt.Length];
                Buffer.BlockCopy(pwdBytes, 0, concat, 0, pwdBytes.Length);
                Buffer.BlockCopy(storedSalt, 0, concat, pwdBytes.Length, storedSalt.Length);
                var computed = sha.ComputeHash(concat);
                return computed.Length == storedHash.Length && computed.SequenceEqual(storedHash);
            }
        }

        public static (byte[] newHash, byte[] newSalt) UpgradeToPBKDF2(string password)
        {
            return HashPasswordPBKDF2(password);
        }
    }
}
