using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ShopFlower.Tests
{
    [TestClass]
    public class PasswordHashingTests
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 10000;

        // Tái tạo logic băm mật khẩu chính xác từ AccountController
        private static (byte[] hash, byte[] salt) HashPassword(string password)
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

        // Tái tạo logic đối sánh PBKDF2 chính xác từ AccountController
        private static bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, storedSalt, Iterations))
            {
                var computed = pbkdf2.GetBytes(HashSize);
                return computed.SequenceEqual(storedHash);
            }
        }

        // Tái tạo logic đối sánh SHA256 cũ chính xác từ AccountController
        private static bool VerifyLegacySHA256Password(string password, byte[] storedHash, byte[] storedSalt)
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

        [TestMethod]
        public void Test_PBKDF2_Hashing_Should_Produce_Valid_Outputs()
        {
            // Arrange
            string password = "SecretPassword@123";

            // Act
            var (hash, salt) = HashPassword(password);

            // Assert
            Assert.IsNotNull(hash, "Bản băm không được null");
            Assert.IsNotNull(salt, "Muối (Salt) không được null");
            Assert.AreEqual(HashSize, hash.Length, $"Độ dài bản băm phải là {HashSize} bytes");
            Assert.AreEqual(SaltSize, salt.Length, $"Độ dài Muối phải là {SaltSize} bytes");
        }

        [TestMethod]
        public void Test_PBKDF2_Verification_Should_Succeed_With_Correct_Password()
        {
            // Arrange
            string password = "MySecurePassword2026";
            var (hash, salt) = HashPassword(password);

            // Act
            bool isVerified = VerifyPassword(password, hash, salt);

            // Assert
            Assert.IsTrue(isVerified, "Xác minh phải thành công khi nhập đúng mật khẩu");
        }

        [TestMethod]
        public void Test_PBKDF2_Verification_Should_Fail_With_Incorrect_Password()
        {
            // Arrange
            string password = "MySecurePassword2026";
            string wrongPassword = "WrongPassword123";
            var (hash, salt) = HashPassword(password);

            // Act
            bool isVerified = VerifyPassword(wrongPassword, hash, salt);

            // Assert
            Assert.IsFalse(isVerified, "Xác minh phải thất bại khi gõ sai mật khẩu");
        }

        [TestMethod]
        public void Test_Legacy_SHA256_Fallback_Should_Authenticate_Legacy_Credentials()
        {
            // Arrange
            string password = "LegacyPassword123";
            var salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Tạo mã băm SHA256 kiểu cũ
            byte[] legacyHash;
            using (var sha = SHA256.Create())
            {
                var pwdBytes = Encoding.UTF8.GetBytes(password);
                var concat = new byte[pwdBytes.Length + salt.Length];
                Buffer.BlockCopy(pwdBytes, 0, concat, 0, pwdBytes.Length);
                Buffer.BlockCopy(salt, 0, concat, pwdBytes.Length, salt.Length);
                legacyHash = sha.ComputeHash(concat);
            }

            // Act
            bool isVerified = VerifyLegacySHA256Password(password, legacyHash, salt);

            // Assert
            Assert.IsTrue(isVerified, "Cơ chế fallback SHA256 phải xác thực thành công thông tin cũ");
        }
    }
}
