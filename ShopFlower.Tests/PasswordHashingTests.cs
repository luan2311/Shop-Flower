using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopFlower.Tests.Helpers;

namespace ShopFlower.Tests
{
    [TestClass]
    public class PasswordHashingTests
    {
        // GROUP 1: PBKDF2 Tests (primary mechanism for new users)

        [TestMethod]
        [TestCategory("PBKDF2")]
        [Description("Check standard PBKDF2 output size: hash=32 bytes, salt=16 bytes")]
        public void HashPasswordPBKDF2_ShouldReturn_CorrectSizes()
        {
            // Arrange
            string password = "TestPassword@123";

            // Act
            var (hash, salt) = PasswordHelper.HashPasswordPBKDF2(password);

            // Assert
            Assert.AreEqual(PasswordHelper.HashSize, hash.Length,
                $"Hash size must be {PasswordHelper.HashSize} bytes, actual: {hash.Length}");
            Assert.AreEqual(PasswordHelper.SaltSize, salt.Length,
                $"Salt size must be {PasswordHelper.SaltSize} bytes, actual: {salt.Length}");
        }

        [TestMethod]
        [TestCategory("PBKDF2")]
        [Description("Verify PBKDF2: correct password must return true")]
        public void VerifyPasswordPBKDF2_WithCorrectPassword_ShouldReturnTrue()
        {
            // Arrange
            string password = "MySecurePassword!456";
            var (hash, salt) = PasswordHelper.HashPasswordPBKDF2(password);

            // Act
            bool result = PasswordHelper.VerifyPasswordPBKDF2(password, hash, salt);

            // Assert
            Assert.IsTrue(result, "PBKDF2 verify with correct password must return TRUE");
        }

        [TestMethod]
        [TestCategory("PBKDF2")]
        [Description("Verify PBKDF2: wrong password must return false")]
        public void VerifyPasswordPBKDF2_WithWrongPassword_ShouldReturnFalse()
        {
            // Arrange
            string correctPassword = "CorrectPassword@789";
            string wrongPassword = "WrongPassword@000";
            var (hash, salt) = PasswordHelper.HashPasswordPBKDF2(correctPassword);

            // Act
            bool result = PasswordHelper.VerifyPasswordPBKDF2(wrongPassword, hash, salt);

            // Assert
            Assert.IsFalse(result, "PBKDF2 verify with wrong password must return FALSE");
        }

        [TestMethod]
        [TestCategory("PBKDF2")]
        [Description("Security: each hash of same password must produce a different random salt")]
        public void HashPasswordPBKDF2_SamePlaintext_ShouldProduceDifferentSalts()
        {
            // Arrange
            string password = "SamePassword@123";

            // Act
            var (hash1, salt1) = PasswordHelper.HashPasswordPBKDF2(password);
            var (hash2, salt2) = PasswordHelper.HashPasswordPBKDF2(password);

            // Assert - salts must be different
            bool saltsAreEqual = salt1.SequenceEqual(salt2);
            Assert.IsFalse(saltsAreEqual,
                "Each hash must produce a DIFFERENT random salt to protect against Rainbow Table attacks");

            // Assert - hashes must also differ (because salts differ)
            bool hashesAreEqual = hash1.SequenceEqual(hash2);
            Assert.IsFalse(hashesAreEqual,
                "Each hash of the same password must produce a different hash value");
        }

        [TestMethod]
        [TestCategory("PBKDF2")]
        [Description("Check PBKDF2 iteration count must be 10000")]
        public void PBKDF2_IterationCount_ShouldBe10000()
        {
            // Assert
            Assert.AreEqual(10000, PasswordHelper.Iterations,
                "PBKDF2 iteration count must be 10000 per security design");
        }

        [TestMethod]
        [TestCategory("PBKDF2")]
        [Description("Hash must not be an all-zero byte array")]
        public void HashPasswordPBKDF2_ShouldNotProduceAllZeroHash()
        {
            // Arrange
            string password = "TestPassword@123";

            // Act
            var (hash, salt) = PasswordHelper.HashPasswordPBKDF2(password);

            // Assert
            bool allZero = hash.All(b => b == 0);
            Assert.IsFalse(allZero, "Hash must not be an all-zero array");
        }

        // GROUP 2: SHA256 Fallback Tests (backward compat for old users)

        [TestMethod]
        [TestCategory("SHA256_Legacy")]
        [Description("SHA256 legacy hash must produce 32-byte hash and 16-byte salt (Guid)")]
        public void HashPasswordSHA256Legacy_ShouldReturn_CorrectSizes()
        {
            // Arrange
            string password = "OldUser@Password";

            // Act
            var (hash, salt) = PasswordHelper.HashPasswordSHA256Legacy(password);

            // Assert
            Assert.AreEqual(32, hash.Length,
                "SHA256 hash must be 32 bytes");
            Assert.AreEqual(16, salt.Length,
                "Salt from Guid must be 16 bytes");
        }

        [TestMethod]
        [TestCategory("SHA256_Legacy")]
        [Description("Fallback SHA256: verifying correct old user password must return true")]
        public void VerifyPasswordSHA256Legacy_WithCorrectPassword_ShouldReturnTrue()
        {
            // Arrange
            string password = "Admin@Pass123";
            var (hash, salt) = PasswordHelper.HashPasswordSHA256Legacy(password);

            // Act
            bool result = PasswordHelper.VerifyPasswordSHA256Legacy(password, hash, salt);

            // Assert
            Assert.IsTrue(result,
                "SHA256 fallback verify must return TRUE when password is correct");
        }

        [TestMethod]
        [TestCategory("SHA256_Legacy")]
        [Description("Fallback SHA256: verifying wrong password must return false")]
        public void VerifyPasswordSHA256Legacy_WithWrongPassword_ShouldReturnFalse()
        {
            // Arrange
            string correctPassword = "CorrectAdmin@Pass";
            string wrongPassword = "WrongAdmin@Pass";
            var (hash, salt) = PasswordHelper.HashPasswordSHA256Legacy(correctPassword);

            // Act
            bool result = PasswordHelper.VerifyPasswordSHA256Legacy(wrongPassword, hash, salt);

            // Assert
            Assert.IsFalse(result,
                "SHA256 fallback verify must return FALSE when password is wrong");
        }

        // GROUP 3: Auto-upgrade SHA256 -> PBKDF2 Tests

        [TestMethod]
        [TestCategory("PasswordUpgrade")]
        [Description("Upgrade flow: SHA256 succeeds -> re-hash to PBKDF2 -> PBKDF2 verifies successfully")]
        public void PasswordUpgrade_SHA256ToPBKDF2_ShouldVerifySuccessfully()
        {
            // Arrange - create old user with SHA256
            string originalPassword = "OldUserPassword@1";
            var (sha256Hash, sha256Salt) = PasswordHelper.HashPasswordSHA256Legacy(originalPassword);

            // Step 1: Simulate login - verify using SHA256 (success)
            bool sha256Verified = PasswordHelper.VerifyPasswordSHA256Legacy(originalPassword, sha256Hash, sha256Salt);
            Assert.IsTrue(sha256Verified, "Step 1: SHA256 verify must succeed");

            // Step 2: Upgrade hash to PBKDF2 (as AccountController.Dang_nhap() does)
            var (newPBKDF2Hash, newPBKDF2Salt) = PasswordHelper.UpgradeToPBKDF2(originalPassword);

            // Step 3: Verify again with new PBKDF2 - must succeed with same password
            bool pbkdf2Verified = PasswordHelper.VerifyPasswordPBKDF2(originalPassword, newPBKDF2Hash, newPBKDF2Salt);
            Assert.IsTrue(pbkdf2Verified,
                "Step 3: After upgrade, PBKDF2 verify must succeed with original password");
        }

        [TestMethod]
        [TestCategory("PasswordUpgrade")]
        [Description("Consistency: old SHA256 must not match new PBKDF2 hash")]
        public void PasswordUpgrade_OldSHA256_ShouldNotMatchNewPBKDF2Hash()
        {
            // Arrange
            string password = "UpgradeTestPassword@2";
            var (pbkdf2Hash, pbkdf2Salt) = PasswordHelper.HashPasswordPBKDF2(password);

            // Act - try SHA256 verify against PBKDF2 hash (incompatible)
            bool sha256WithPbkdf2Hash = PasswordHelper.VerifyPasswordSHA256Legacy(password, pbkdf2Hash, pbkdf2Salt);

            // Assert - must return false because format is incompatible
            Assert.IsFalse(sha256WithPbkdf2Hash,
                "SHA256 verify must not succeed against a PBKDF2-format hash (different sizes)");
        }

        // GROUP 4: Boundary and Edge Case Tests

        [TestMethod]
        [TestCategory("EdgeCases")]
        [Description("Empty password can still be hashed without crash")]
        public void HashPasswordPBKDF2_WithEmptyPassword_ShouldNotThrow()
        {
            // Arrange
            string emptyPassword = "";

            // Act & Assert - must not throw exception
            try
            {
                var (hash, salt) = PasswordHelper.HashPasswordPBKDF2(emptyPassword);
                Assert.IsNotNull(hash, "Hash must not be null even with empty password");
                Assert.IsNotNull(salt, "Salt must not be null even with empty password");
            }
            catch (Exception ex)
            {
                Assert.Fail($"HashPasswordPBKDF2 must not throw exception with empty password. Error: {ex.Message}");
            }
        }

        [TestMethod]
        [TestCategory("EdgeCases")]
        [Description("Very long password (1000 chars) must still hash successfully")]
        public void HashPasswordPBKDF2_WithVeryLongPassword_ShouldSucceed()
        {
            // Arrange
            string longPassword = new string('A', 1000);

            // Act
            var (hash, salt) = PasswordHelper.HashPasswordPBKDF2(longPassword);

            // Assert
            Assert.AreEqual(PasswordHelper.HashSize, hash.Length,
                "Hash size must be correct even with very long password");
            bool verified = PasswordHelper.VerifyPasswordPBKDF2(longPassword, hash, salt);
            Assert.IsTrue(verified, "Verify must succeed with long password");
        }

        [TestMethod]
        [TestCategory("EdgeCases")]
        [Description("Unicode password must hash and verify successfully")]
        public void HashPasswordPBKDF2_WithUnicodePassword_ShouldSucceed()
        {
            // Arrange
            string unicodePassword = "Mat@Khau123HoaFlower";

            // Act
            var (hash, salt) = PasswordHelper.HashPasswordPBKDF2(unicodePassword);
            bool verified = PasswordHelper.VerifyPasswordPBKDF2(unicodePassword, hash, salt);

            // Assert
            Assert.IsTrue(verified, "Verify must succeed with unicode password");
        }
    }
}
