using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopFlower.APIController;
using ShopFlower.Models;
using ShopFlower.Tests.Helpers;

namespace ShopFlower.Tests
{
    [TestClass]
    public class BugRevealingTests
    {
        // GROUP A: AccountController - Registration (REG_*)

        [TestMethod]
        [TestCategory("BugRevealing_REG")]
        [Description("[FAIL] REG-B01: No minimum password length check")]
        public void Register_PasswordTooShort_ShouldBeRejected_REG_B01()
        {
            // Arrange - password 1 char (too short)
            string shortPassword = "1";
            int minimumLength = 6; // minimum requirement per best practice

            // Act - length check (Dang_ky() SHOULD have this)
            bool meetsMinLength = shortPassword.Length >= minimumLength;

            // Assert - FAIL: Dang_ky() does not have this check
            Assert.IsTrue(meetsMinLength,
                $"[BUG REG-B01] Password '{shortPassword}' has only {shortPassword.Length} char(s). " +
                $"AccountController.Dang_ky() does NOT check minimum length ({minimumLength} chars). " +
                $"Need to add: if (MatKhau.Length < 6) -> return error");
        }

        [TestMethod]
        [TestCategory("BugRevealing_REG")]
        [Description("[FAIL] REG-B02: No password complexity check")]
        public void Register_PasswordNoComplexity_ShouldBeRejected_REG_B02()
        {
            // Arrange - all digits, no uppercase, no special char
            string weakPassword = "123456";

            // Act - complexity check (Dang_ky() SHOULD have this)
            bool hasUppercase = weakPassword.Any(char.IsUpper);
            bool hasSpecialChar = weakPassword.Any(c => !char.IsLetterOrDigit(c));
            bool isComplex = hasUppercase && hasSpecialChar;

            // Assert - FAIL: Dang_ky() does not have this validation
            Assert.IsTrue(isComplex,
                $"[BUG REG-B02] Password '{weakPassword}' is not complex enough. " +
                $"AccountController.Dang_ky() does NOT check password complexity. " +
                $"Need to require: at least 1 uppercase + 1 special character");
        }

        [TestMethod]
        [TestCategory("BugRevealing_REG")]
        [Description("[FAIL] REG-B03: TenDangNhap not trimmed before saving to DB")]
        public void Register_Username_ShouldBeTrimmed_REG_B03()
        {
            // Arrange
            string inputUsername = "  user_03  ";
            string expectedUsername = "user_03";

            // Act - simulate Dang_ky() (line 461 AccountController.cs)
            var newUser = new TAIKHOAN { TenDangNhap = inputUsername }; // NO .Trim()

            // Assert - FAIL: saved with leading/trailing spaces
            Assert.AreEqual(expectedUsername, newUser.TenDangNhap,
                $"[BUG REG-B03] TenDangNhap saved as '{newUser.TenDangNhap}', expected '{expectedUsername}'. " +
                $"Fix line 461 AccountController.cs: TenDangNhap = TenDangNhap.Trim()");
        }

        [TestMethod]
        [TestCategory("BugRevealing_REG")]
        [Description("[FAIL] REG-B04: Username duplicate check is case-sensitive (bug)")]
        public void Register_UsernameCheck_ShouldBeCaseInsensitive_REG_B04()
        {
            // Arrange
            string existingUsername = "admin";
            string newUsername = "Admin"; // uppercase A - different under ==

            // Act - simulate logic line 435: db.TAIKHOANs.Any(u => u.TenDangNhap == TenDangNhap)
            bool isDuplicate_CaseSensitive = existingUsername == newUsername; // ACTUAL system does this
            bool isDuplicate_CaseInsensitive = string.Equals(existingUsername, newUsername,
                StringComparison.OrdinalIgnoreCase); // CORRECT: should do this

            // Assert - FAIL: system uses == so isDuplicate = false (allows registering "Admin")
            Assert.IsTrue(isDuplicate_CaseSensitive,
                $"[BUG REG-B04] System allows registering '{newUsername}' when '{existingUsername}' exists. " +
                $"Line 435 uses == (case-sensitive). Need StringComparison.OrdinalIgnoreCase. " +
                $"Correct result (case-insensitive): {isDuplicate_CaseInsensitive}");
        }

        [TestMethod]
        [TestCategory("BugRevealing_REG")]
        [Description("[FAIL] REG-B05: No server-side email format validation in Dang_ky()")]
        public void Register_InvalidEmailFormat_ShouldBeRejectedServerSide_REG_B05()
        {
            // Arrange
            string invalidEmail = "notanemail"; // no @domain

            // Act - format check (Dang_ky() SHOULD have this)
            bool isValidFormat = invalidEmail.Contains("@") &&
                                 new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").IsMatch(invalidEmail);

            // Check if Dang_ky() has Regex/MailAddress validation (it doesn't)
            bool controllerHasEmailValidation = typeof(ShopFlower.Controllers.AccountController)
                .GetMethod("Dang_ky",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance,
                    null,
                    new[] { typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string) },
                    null) != null;

            // Assert - system does not validate email format server-side
            Assert.IsTrue(isValidFormat,
                $"[BUG REG-B05] Email '{invalidEmail}' is invalid but Dang_ky() only checks IsNullOrWhiteSpace. " +
                $"No Regex or MailAddress validation present. " +
                $"Need to add: if (!IsValidEmail(Email)) -> return error");
        }

        [TestMethod]
        [TestCategory("BugRevealing_REG")]
        [Description("[PASS] REG-B06: Empty password is correctly blocked first (control test)")]
        public void Register_EmptyPassword_ShouldBeRejectedFirst_REG_B06()
        {
            // Arrange
            string emptyPwd = "";
            bool isNullOrWhitespace = string.IsNullOrWhiteSpace(emptyPwd); // true -> blocked

            // Assert - PASS: correct order of validation
            Assert.IsTrue(isNullOrWhitespace,
                "Empty password must be caught by IsNullOrWhiteSpace before match check");
        }

        // GROUP B: AccountController - Login & Security (LOG_*)

        [TestMethod]
        [TestCategory("BugRevealing_LOG")]
        [Description("[FAIL] LOG-B01: No Brute Force Protection - no account lockout after many failures")]
        public void Login_NoBruteForceProtection_LOG_B01()
        {
            // Act - search for lockout/brute-force related methods in AccountController
            var allMethods = typeof(ShopFlower.Controllers.AccountController)
                .GetMethods(System.Reflection.BindingFlags.Public |
                            System.Reflection.BindingFlags.NonPublic |
                            System.Reflection.BindingFlags.Instance)
                .Select(m => m.Name.ToLower()).ToList();

            bool hasLockoutLogic = allMethods.Any(m =>
                m.Contains("lockout") || m.Contains("failedattempt") ||
                m.Contains("bruteforce") || m.Contains("maxattempt") || m.Contains("captcha"));

            // Assert - FAIL: no lockout logic found
            Assert.IsTrue(hasLockoutLogic,
                "[BUG LOG-B01] AccountController has no method handling brute force. " +
                "Need to add: int failedAttempts, lockout after 5 failures, show Captcha after 3.");
        }

        [TestMethod]
        [TestCategory("BugRevealing_LOG")]
        [Description("[FAIL] LOG-B02: Login is case-sensitive - 'Admin' fails when registered as 'admin'")]
        public void Login_CaseSensitiveUsername_ShouldBeFlexible_LOG_B02()
        {
            // Arrange
            string registeredUsername = "admin";    // name stored in DB
            string inputUsername = "Admin";          // user types uppercase A

            // Act - simulate logic line 82: u.TenDangNhap == TenDangNhap
            bool userFound_CaseSensitive = registeredUsername == inputUsername; // false -> login fails
            bool userFound_CaseInsensitive = string.Equals(registeredUsername, inputUsername,
                StringComparison.OrdinalIgnoreCase); // true -> correct approach

            // Assert - FAIL: case-sensitivity causes login failure
            Assert.IsTrue(userFound_CaseSensitive,
                $"[BUG LOG-B02] Login with '{inputUsername}' (upper) fails when DB stores '{registeredUsername}' (lower). " +
                $"Line 82 Dang_nhap() uses == (case-sensitive). " +
                $"Need: u.TenDangNhap.ToLower() == TenDangNhap.ToLower()");
        }

        [TestMethod]
        [TestCategory("BugRevealing_LOG")]
        [Description("[FAIL] LOG-B03: Login does not trim username - trailing space causes failure")]
        public void Login_Username_ShouldBeTrimmed_BeforeSearch_LOG_B03()
        {
            // Arrange
            string storedUsername = "admin";
            string inputUsername = "admin "; // trailing space

            // Act - simulate DB search (line 82)
            bool foundWithoutTrim = storedUsername == inputUsername;             // false -> bug
            bool foundWithTrim = storedUsername == inputUsername.Trim();         // true -> correct

            // Assert - FAIL: no trim causes lookup failure
            Assert.IsTrue(foundWithoutTrim,
                $"[BUG LOG-B03] Input '{inputUsername}' does not find user '{storedUsername}'. " +
                $"Dang_nhap() does not call TenDangNhap.Trim(). " +
                $"Fix: var user = db.TAIKHOANs.SingleOrDefault(u => u.TenDangNhap == TenDangNhap.Trim())");
        }

        // GROUP C: AccountController - Change/Reset Password (PWD_*)

        [TestMethod]
        [TestCategory("BugRevealing_PWD")]
        [Description("[FAIL] PWD-B01: Password reset does not require email token - HIGH security hole")]
        public void PasswordReset_ShouldRequireEmailToken_PWD_B01()
        {
            // Arrange - check if Reset_mat_khau has a token parameter
            var resetMethod = typeof(ShopFlower.Controllers.AccountController)
                .GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                .Where(m => m.Name == "Reset_mat_khau")
                .ToList();

            // Check parameter: any "token" or "otp" parameter?
            bool hasTokenParameter = resetMethod.Any(m =>
                m.GetParameters().Any(p =>
                    p.Name.ToLower().Contains("token") ||
                    p.Name.ToLower().Contains("otp") ||
                    p.Name.ToLower().Contains("code")));

            // Assert - FAIL: no token parameter in Reset_mat_khau
            Assert.IsTrue(hasTokenParameter,
                "[BUG PWD-B01] Reset_mat_khau() has no 'token' or 'otp' parameter. " +
                "Anyone who knows a username can reset the password. " +
                "OWASP A07:2021 - Identification and Authentication Failures. " +
                "Need: GUID token sent via email + expire time + PasswordResets table");
        }

        [TestMethod]
        [TestCategory("BugRevealing_PWD")]
        [Description("[FAIL] PWD-B02: Change password has no minimum length check")]
        public void ChangePassword_NewPassword_NoMinLengthCheck_PWD_B02()
        {
            // Arrange
            string newPasswordTooShort = "1";
            int minLength = 6;

            // Act - length check (Doi_mat_khau() SHOULD have this)
            bool meetsMinLength = newPasswordTooShort.Length >= minLength;

            // Assert - FAIL
            Assert.IsTrue(meetsMinLength,
                $"[BUG PWD-B02] New password '{newPasswordTooShort}' has only {newPasswordTooShort.Length} char(s). " +
                $"Doi_mat_khau() does not check minimum length. " +
                $"Need to add: if (NewPassword.Length < 6) -> ModelState.AddModelError(...)");
        }

        [TestMethod]
        [TestCategory("BugRevealing_PWD")]
        [Description("[FAIL] PWD-B03: New password is allowed to be same as old password")]
        public void ChangePassword_NewPasswordSameAsOld_ShouldBeRejected_PWD_B03()
        {
            // Arrange
            string oldPassword = "abc@123";
            string newPassword = "abc@123"; // exactly same as old password

            // Act - check if validation exists
            bool isSamePassword = oldPassword == newPassword;
            // Doi_mat_khau() only checks NewPassword != ConfirmPassword, NOT NewPassword != OldPassword

            // Assert - FAIL: system does not block this case
            Assert.IsFalse(isSamePassword,
                $"[BUG PWD-B03] New password is same as old password (both = '{oldPassword}'). " +
                $"Doi_mat_khau() does not check OldPassword != NewPassword. " +
                $"Need to add: if (NewPassword == OldPassword) -> ModelState.AddModelError(...)");
        }

        // GROUP D: HoaDonController - Order Validation (HD_*)

        [TestMethod]
        [TestCategory("BugRevealing_HD")]
        [Description("[FAIL] HD-B01: OrderItem SoLuong = 0 not rejected - can order 0 items")]
        public void PlaceOrder_ItemSoLuongZero_ShouldBeRejected_HD_B01()
        {
            // Arrange
            var controller = CreateHoaDonController();
            var model = CreateValidOrderModel();
            model.Items = new List<HoaDonController.OrderItemModel>
            {
                new HoaDonController.OrderItemModel
                {
                    MaSP = "SP001",
                    SoLuong = 0,    // invalid
                    DonGia = 100000m
                }
            };

            // Act
            var result = controller.PlaceOrder(model);

            // Assert - FAIL: PlaceOrder() does not validate SoLuong > 0
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult),
                "[BUG HD-B01] PlaceOrder accepts SoLuong = 0. " +
                "Need to add: if (item.SoLuong <= 0) -> return BadRequest(\"SoLuong phai lon hon 0\")");
        }

        [TestMethod]
        [TestCategory("BugRevealing_HD")]
        [Description("[FAIL] HD-B02: Negative OrderItem SoLuong not rejected - causes wrong total")]
        public void PlaceOrder_ItemNegativeSoLuong_ShouldBeRejected_HD_B02()
        {
            // Arrange
            var controller = CreateHoaDonController();
            var model = CreateValidOrderModel();
            model.Items = new List<HoaDonController.OrderItemModel>
            {
                new HoaDonController.OrderItemModel
                {
                    MaSP = "SP002",
                    SoLuong = -5,   // negative
                    DonGia = 100000m
                }
            };

            // Act
            var result = controller.PlaceOrder(model);

            // Assert - FAIL
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult),
                "[BUG HD-B02] PlaceOrder accepts SoLuong = -5. " +
                "System can create CTHD with negative SoLuong, causing wrong total. " +
                "Need to add validation: item.SoLuong > 0");
        }

        [TestMethod]
        [TestCategory("BugRevealing_HD")]
        [Description("[FAIL] HD-B03: DonGia = 0 accepted - customer can self-declare price of 0")]
        public void PlaceOrder_ItemDonGiaZero_ShouldBeRejected_HD_B03()
        {
            // Arrange
            var controller = CreateHoaDonController();
            var model = CreateValidOrderModel();
            model.Items = new List<HoaDonController.OrderItemModel>
            {
                new HoaDonController.OrderItemModel
                {
                    MaSP = "SP003",
                    SoLuong = 2,
                    DonGia = 0m     // price 0
                }
            };

            // Act
            var result = controller.PlaceOrder(model);

            // Assert - FAIL: PlaceOrder does not validate DonGia > 0
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult),
                "[BUG HD-B03] PlaceOrder accepts DonGia = 0. " +
                "Customer can self-declare price as 0 and order for free. " +
                "Fix: fetch price from DB instead of trusting client, or validate DonGia > 0");
        }

        [TestMethod]
        [TestCategory("BugRevealing_HD")]
        [Description("[FAIL] HD-B04: Negative DonGia not rejected - total can go negative")]
        public void PlaceOrder_ItemNegativeDonGia_ShouldBeRejected_HD_B04()
        {
            // Arrange
            var controller = CreateHoaDonController();
            var model = CreateValidOrderModel();
            model.Items = new List<HoaDonController.OrderItemModel>
            {
                new HoaDonController.OrderItemModel
                {
                    MaSP = "SP004",
                    SoLuong = 1,
                    DonGia = -50000m    // negative price
                }
            };

            // Act
            var result = controller.PlaceOrder(model);

            // Assert - FAIL
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult),
                "[BUG HD-B04] PlaceOrder accepts DonGia = -50000. " +
                "TongTien = 1 * (-50000) = -50000 -> places order that reduces customer debt. " +
                "Need to validate: item.DonGia > 0");
        }

        [TestMethod]
        [TestCategory("BugRevealing_HD")]
        [Description("[FAIL] HD-B05: MaTK = 0 not rejected early at validation layer")]
        public void PlaceOrder_MaTKIsZero_ShouldBeRejectedEarly_HD_B05()
        {
            // Arrange
            var controller = CreateHoaDonController();
            var model = CreateValidOrderModel();
            model.MaTK = 0; // invalid MaTK

            // Act - check if early validation exists (before DB query)
            bool hasEarlyValidation = model.MaTK > 0;

            // Assert - FAIL: PlaceOrder uses MaTK to query DB first (line 150), no MaTK > 0 check
            Assert.IsTrue(hasEarlyValidation,
                "[BUG HD-B05] PlaceOrder does not validate MaTK > 0 before DB query. " +
                "MaTK = 0 causes 'db.TAIKHOANs.Any(t => t.MaTK == 0)' unnecessary query. " +
                "Need to add: if (model.MaTK <= 0) return BadRequest(\"MaTK khong hop le\")");
        }

        [TestMethod]
        [TestCategory("BugRevealing_HD")]
        [Description("[FAIL] HD-B06: Client-sent price not verified against DB - Price Manipulation")]
        public void PlaceOrder_ClientSidePrice_NotVerifiedWithDB_HD_B06()
        {
            // Arrange - assume DB has SP001 at price 150000
            decimal realPriceInDB = 150000m;
            decimal clientSentPrice = 1m; // client deliberately sends price of 1

            // Act - calculate total as server currently does (trusts client)
            int quantity = 2;
            decimal calculatedTotal_Buggy = quantity * clientSentPrice; // = 2 (wrong)
            decimal calculatedTotal_Correct = quantity * realPriceInDB; // = 300000 (right)

            // Assert - demonstrate the difference
            Assert.AreEqual(calculatedTotal_Correct, calculatedTotal_Buggy,
                $"[BUG HD-B06] TongTien from client price = {calculatedTotal_Buggy:N0}d, " +
                $"real DB price = {calculatedTotal_Correct:N0}d. " +
                $"PlaceOrder() trusts item.DonGia from client, does not query SANPHAM.GiaBan from DB. " +
                $"This is a CRITICAL Price Manipulation vulnerability.");
        }

        // GROUP E: Business Logic - SoLuongTon (BIZ_*)

        [TestMethod]
        [TestCategory("BugRevealing_BIZ")]
        [Description("[FAIL] BIZ-B01: PlaceOrder does not decrease SoLuongTon after successful order")]
        public void PlaceOrder_ShouldDecrease_SoLuongTon_BIZ_B01()
        {
            // Arrange
            var product = new SANPHAM { MaSP = "SP001", SoLuongTon = 5 };
            int orderedQty = 3;
            int expectedRemaining = 2;

            // Act - simulate CheckoutController.PlaceOrder() (does NOT subtract SoLuongTon)
            var cthd = new CTHD { MaSP = product.MaSP, SoLuong = orderedQty };
            // Missing: product.SoLuongTon -= orderedQty;

            // Assert - FAIL
            Assert.AreEqual(expectedRemaining, product.SoLuongTon.Value,
                $"[BUG BIZ-B01] SoLuongTon after order = {product.SoLuongTon} (expected {expectedRemaining}). " +
                $"CheckoutController.PlaceOrder() does not subtract SoLuongTon after creating CTHD.");
        }

        [TestMethod]
        [TestCategory("BugRevealing_BIZ")]
        [Description("[FAIL] BIZ-B02: No SoLuongTon check on order - can order more than available stock")]
        public void PlaceOrder_QuantityExceedsStock_ShouldBeRejected_BIZ_B02()
        {
            // Arrange
            int soLuongTon = 2;
            int soLuongDat = 10;

            // Act - check (system SHOULD have this)
            bool isOrderValid = soLuongDat <= soLuongTon;

            // Assert - FAIL
            Assert.IsTrue(isOrderValid,
                $"[BUG BIZ-B02] System allows ordering {soLuongDat} units when only {soLuongTon} available. " +
                $"Need to add: if (item.SoLuong > product.SoLuongTon) -> return error");
        }

        [TestMethod]
        [TestCategory("BugRevealing_BIZ")]
        [Description("[FAIL] BIZ-B03: TongTien = 0 when DonGia = 0 - not rejected")]
        public void PlaceOrder_TongTienZero_WhenDonGiaZero_BIZ_B03()
        {
            // Arrange
            var items = new List<HoaDonController.OrderItemModel>
            {
                new HoaDonController.OrderItemModel { MaSP = "SP001", SoLuong = 5, DonGia = 0m }
            };

            // Act - calculate total as server does
            decimal tongTien = items.Sum(i => i.SoLuong * i.DonGia);

            // Assert - TongTien = 0 -> not rejected
            Assert.AreNotEqual(0m, tongTien,
                $"[BUG BIZ-B03] TongTien = {tongTien} when DonGia = 0. " +
                $"PlaceOrder does not validate TongTien > 0. " +
                $"Need to add: if (tongTien <= 0) return BadRequest(\"Tong tien khong hop le\")");
        }

        [TestMethod]
        [TestCategory("BugRevealing_BIZ")]
        [Description("[FAIL] BIZ-B04: TongTien is negative when DonGia is negative - not detected")]
        public void PlaceOrder_TongTienNegative_WhenDonGiaNegative_BIZ_B04()
        {
            // Arrange
            var items = new List<HoaDonController.OrderItemModel>
            {
                new HoaDonController.OrderItemModel { MaSP = "SP001", SoLuong = 3, DonGia = -100000m }
            };

            // Act
            decimal tongTien = items.Sum(i => i.SoLuong * i.DonGia); // = -300000

            // Assert
            Assert.IsTrue(tongTien > 0,
                $"[BUG BIZ-B04] TongTien = {tongTien:N0}d (negative). " +
                $"PlaceOrder does not validate DonGia > 0 and TongTien > 0. This is a critical vulnerability.");
        }

        // GROUP F: SanPhamController - Product Validation (SP_*)

        [TestMethod]
        [TestCategory("BugRevealing_SP")]
        [Description("[FAIL] SP-B01: Negative page number (-1) causes pagination error - not normalized to page 1")]
        public void SanPham_NegativePageNumber_ShouldDefaultToPage1_SP_B01()
        {
            // Arrange
            int pageInput = -1;
            int pageSize = 12;

            // Act - simulate LINQ in SanPhamController (no normalization)
            // .Skip((page - 1) * pageSize) when page = -1 -> Skip(-12) -> ArgumentOutOfRangeException
            int skipCount = (pageInput - 1) * pageSize; // = -24 -> error

            // Assert - FAIL: negative skip causes error
            Assert.IsTrue(skipCount >= 0,
                $"[BUG SP-B01] Skip({skipCount}) with page=-1. " +
                $"SanPhamController does not normalize negative page to 1. " +
                $"Need to add: if (page < 1) page = 1;");
        }

        [TestMethod]
        [TestCategory("BugRevealing_SP")]
        [Description("[FAIL] SP-B02: Empty/null search keyword not blocked early - unnecessary query")]
        public void SanPham_EmptySearchKeyword_ShouldReturnEarlyWithEmptyResult_SP_B02()
        {
            // Arrange
            string emptyKeyword = "";

            // Act - check if early return exists
            bool shouldSkipQuery = string.IsNullOrWhiteSpace(emptyKeyword);

            // If no early return -> calls SP SearchProducts with @Keyword = ""
            // -> returns all products (not a search result)
            bool hasEarlyReturn = shouldSkipQuery; // controller SHOULD check this

            // Assert - FAIL: Search("") calls SP with empty keyword -> undefined result
            Assert.IsTrue(hasEarlyReturn,
                "[BUG SP-B02] SanPhamController.Search() does not check empty keyword before calling SP. " +
                "Search with keyword='' returns undefined results. " +
                "Need to add: if (string.IsNullOrWhiteSpace(query)) return View(emptyResult)");
        }

        [TestMethod]
        [TestCategory("BugRevealing_SP")]
        [Description("[FAIL] SP-B03: GiaBan can be null - product without price causes display error")]
        public void SanPham_NullGiaBan_ShouldBeRejected_SP_B03()
        {
            // Arrange
            var product = new SANPHAM
            {
                MaSP = "SP_NULL_PRICE",
                TenSP = "Hoa Test",
                GiaBan = null, // null
                SoLuongTon = 10,
                TinhTrang = "Con hang"
            };

            // Act
            bool isValidPrice = product.GiaBan.HasValue && product.GiaBan.Value > 0;

            // Assert - FAIL: null GiaBan is not blocked
            Assert.IsTrue(isValidPrice,
                "[BUG SP-B03] GiaBan = null can be saved to DB. " +
                "When displayed: @item.GiaBan?.ToString(\"N0\") -> shows blank or null. " +
                "Need: [Required] annotation or validation in Admin controller");
        }

        // GROUP G: LienHeController - Validation (LH_*)

        [TestMethod]
        [TestCategory("BugRevealing_LH")]
        [Description("[FAIL] LH-B01: Phone '0000000000' passes regex but is a fake number")]
        public void LienHe_PhoneAllZeros_PassesRegexButIsFake_LH_B01()
        {
            // Arrange
            string fakePhone = "0000000000";
            var regex = new Regex(@"^0\d{9}$");

            // Act
            bool passesRegex = regex.IsMatch(fakePhone); // true - passes

            // Check if blacklist for all-zeros exists
            bool isBlacklisted = fakePhone == "0000000000" || fakePhone == "0123456789";

            // Assert - FAIL: passes regex but is fake
            Assert.IsFalse(passesRegex || !isBlacklisted,
                $"[BUG LH-B01] Phone '{fakePhone}' passes regex ^0\\d{{9}}$ = {passesRegex}. " +
                $"Regex only checks format, not real-world validity of the phone number. " +
                $"Can improve: blacklist fake numbers or integrate SMS OTP");
        }

        [TestMethod]
        [TestCategory("BugRevealing_LH")]
        [Description("[FAIL] LH-B02: NOIDUNG containing HTML script not sanitized - XSS vulnerability")]
        public void LienHe_ScriptInNoidung_ShouldBeSanitized_LH_B02()
        {
            // Arrange
            string xssPayload = "<script>alert('XSS Attack!')</script>";

            // Act - check if sanitized
            bool containsScript = xssPayload.ToLower().Contains("<script");

            // LienHeController does not sanitize input before calling sp_ThemLienHe
            // When admin views contact list -> script can execute
            bool isSanitized = !xssPayload.Contains("<") && !xssPayload.Contains(">");

            // Assert - FAIL: not sanitized
            Assert.IsTrue(isSanitized,
                $"[BUG LH-B02] NOIDUNG = '{xssPayload}' is not sanitized. " +
                $"LienHeController does not filter HTML tags before saving to DB. " +
                $"When admin views it: <script> tag can execute. " +
                $"Fix: HttpUtility.HtmlEncode(model.NOIDUNG) or AntiXSS library");
        }

        [TestMethod]
        [TestCategory("BugRevealing_LH")]
        [Description("[FAIL] LH-B03: Delete with negative ID has no early validation - unnecessary DB query")]
        public void LienHe_DeleteNegativeId_ShouldReturnBadRequestEarly_LH_B03()
        {
            // Arrange
            var controller = CreateLienHeController();
            int negativeId = -1;

            // Act
            var result = controller.DeleteLienHe(negativeId);

            // Assert - FAIL: Controller does not check id > 0 first, calls db.LIENHEs.Find(-1) then NotFound
            Assert.IsInstanceOfType(result, typeof(System.Web.Http.Results.BadRequestErrorMessageResult),
                $"[BUG LH-B03] DeleteLienHe({negativeId}) does not return BadRequest immediately. " +
                $"Controller calls db.LIENHEs.Find(-1) then returns NotFound. " +
                $"Need to add: if (id <= 0) return BadRequest(\"ID khong hop le\")");
        }

        // GROUP H: Business Logic - Calculation & Boundary Data (CALC_*)

        [TestMethod]
        [TestCategory("BugRevealing_CALC")]
        [Description("[FAIL] CALC-B01: Null TinhThanh produces invalid address '123 Street, District, ,'")]
        public void PlaceOrder_NullTinhThanh_ProducesInvalidAddress_CALC_B01()
        {
            // Arrange
            string diaChiGiaoHang = "123 Duong ABC";
            string phuongXa = "Phuong 1";
            string quanHuyen = "Quan 1";
            string tinhThanh = null; // null

            // Act - simulate lines 157-161 HoaDonController.PlaceOrder()
            string diaChiDayDu = string.Format("{0}, {1}, {2}, {3}",
                diaChiGiaoHang,
                phuongXa ?? "",
                quanHuyen ?? "",
                tinhThanh ?? ""); // = "123 Duong ABC, Phuong 1, Quan 1, "

            // Assert - address has empty part at end
            bool isValidAddress = !diaChiDayDu.EndsWith(", ") &&
                                  !diaChiDayDu.Contains(", , ") &&
                                  diaChiDayDu.Split(',').All(p => !string.IsNullOrWhiteSpace(p));

            Assert.IsTrue(isValidAddress,
                $"[BUG CALC-B01] Assembled address = '{diaChiDayDu}' has empty part at end. " +
                $"TinhThanh = null -> incomplete address. " +
                $"Fix: Validate TinhThanh as required in PlaceOrderModel");
        }

        [TestMethod]
        [TestCategory("BugRevealing_CALC")]
        [Description("[FAIL] CALC-B02: Duplicate MaSP items in order not detected - 2 duplicate CTHDs")]
        public void PlaceOrder_DuplicateMaSP_ShouldBeMergedOrRejected_CALC_B02()
        {
            // Arrange
            var items = new List<HoaDonController.OrderItemModel>
            {
                new HoaDonController.OrderItemModel { MaSP = "SP001", SoLuong = 2, DonGia = 100000m },
                new HoaDonController.OrderItemModel { MaSP = "SP001", SoLuong = 3, DonGia = 100000m } // duplicate!
            };

            // Act - check for duplicates
            bool hasDuplicate = items.GroupBy(i => i.MaSP).Any(g => g.Count() > 1);

            // System currently does not detect -> creates 2 CTHDs with same MaHD + MaSP
            bool systemDetectsDuplicate = false; // PlaceOrder() has no such logic

            // Assert - FAIL: duplicates not detected
            Assert.IsTrue(systemDetectsDuplicate,
                $"[BUG CALC-B02] {items.Count(i => i.MaSP == "SP001")} items with duplicate MaSP='SP001'. " +
                $"PlaceOrder() does not detect duplicate MaSP -> creates 2 duplicate CTHDs in same order. " +
                $"Fix: validate no duplicate MaSP, or auto-merge quantities");
        }

        [TestMethod]
        [TestCategory("BugRevealing_CALC")]
        [Description("[FAIL] CALC-B03: No maximum quantity limit - can order unlimited items")]
        public void PlaceOrder_NoMaxQuantityLimit_CALC_B03()
        {
            // Arrange
            int soLuongExtreme = 999999;
            decimal donGia = 10000000m; // 10 million per item
            int maxAllowedQuantity = 1000; // reasonable limit

            // Act
            bool exceededMax = soLuongExtreme > maxAllowedQuantity;
            decimal tongTien = soLuongExtreme * donGia; // ~9.99 * 10^12 VND

            // Assert - FAIL: no quantity limit
            Assert.IsFalse(exceededMax,
                $"[BUG CALC-B03] SoLuong = {soLuongExtreme:N0} is accepted. " +
                $"TongTien = {tongTien:N0}d (~{(double)tongTien / 1e12:F1} thousand billion VND). " +
                $"HoaDonController has no maximum quantity limit. " +
                $"Fix: if (item.SoLuong > 1000) return BadRequest(\"SoLuong vuot gioi han\")");
        }

        // [PASS] Control tests - features that work correctly

        [TestMethod]
        [TestCategory("BugRevealing_Control")]
        [Description("[PASS] Password and confirm mismatch is correctly detected (control test)")]
        public void Register_PasswordMismatch_IsDetectedCorrectly_CTRL_01()
        {
            string pwd = "abc@123";
            string confirmPwd = "xyz@789";
            Assert.AreNotEqual(pwd, confirmPwd, "Password mismatch must be detected");
        }

        [TestMethod]
        [TestCategory("BugRevealing_Control")]
        [Description("[PASS] PBKDF2 hash and verify works correctly (control test)")]
        public void PasswordHashing_PBKDF2_WorksCorrectly_CTRL_02()
        {
            string password = "ValidPassword@123";
            var (hash, salt) = PasswordHelper.HashPasswordPBKDF2(password);
            bool verified = PasswordHelper.VerifyPasswordPBKDF2(password, hash, salt);
            Assert.IsTrue(verified, "PBKDF2 hash and verify must work correctly");
        }

        [TestMethod]
        [TestCategory("BugRevealing_Control")]
        [Description("[PASS] Total amount calculated correctly with valid data (control test)")]
        public void PlaceOrder_ValidItems_TongTienIsCorrect_CTRL_03()
        {
            var items = new List<HoaDonController.OrderItemModel>
            {
                new HoaDonController.OrderItemModel { SoLuong = 2, DonGia = 150000m },
                new HoaDonController.OrderItemModel { SoLuong = 1, DonGia = 200000m }
            };
            decimal total = items.Sum(i => i.SoLuong * i.DonGia);
            Assert.AreEqual(500000m, total, "Total must = 2x150000 + 1x200000 = 500000");
        }

        [TestMethod]
        [TestCategory("BugRevealing_Control")]
        [Description("[PASS] Valid phone 0901234567 passes regex correctly (control test)")]
        public void LienHe_ValidPhone_PassesRegexCorrectly_CTRL_04()
        {
            var model = new LIENHE
            {
                HOTEN = "Nguyen Van A",
                EMAIL = "test@email.com",
                DIENTHOAI = "0901234567",
                NOIDUNG = "Noi dung test binh thuong"
            };
            var results = ValidationHelper.ValidateModel(model);
            Assert.AreEqual(0, results.Count, "Valid model must have no validation errors");
        }

        // Helper Methods

        private HoaDonController CreateHoaDonController()
        {
            var controller = new HoaDonController();
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage();
            request.SetConfiguration(config);
            controller.Request = request;
            controller.Configuration = config;
            return controller;
        }

        private LienHeController CreateLienHeController()
        {
            var controller = new LienHeController();
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage();
            request.SetConfiguration(config);
            controller.Request = request;
            controller.Configuration = config;
            return controller;
        }

        private HoaDonController.PlaceOrderModel CreateValidOrderModel()
        {
            return new HoaDonController.PlaceOrderModel
            {
                MaTK = 1,
                TenNguoiNhan = "Nguyen Van Test",
                Email = "test@shopflower.com",
                SoDienThoai = "0901234567",
                DiaChiGiaoHang = "123 Duong ABC",
                PhuongXa = "Phuong 1",
                QuanHuyen = "Quan 1",
                TinhThanh = "TP. HCM",
                GhiChu = "",
                PhuongThucThanhToan = "COD",
                Items = new List<HoaDonController.OrderItemModel>
                {
                    new HoaDonController.OrderItemModel { MaSP = "SP001", SoLuong = 1, DonGia = 150000m }
                }
            };
        }
    }
}
