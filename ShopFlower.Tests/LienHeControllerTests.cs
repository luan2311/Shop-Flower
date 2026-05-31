using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShopFlower.APIController;
using ShopFlower.Models;

namespace ShopFlower.Tests
{
    [TestClass]
    public class LienHeControllerTests
    {
        // GROUP 1: Validation Logic Tests (no DB needed)

        [TestMethod]
        [TestCategory("LienHe_Validation")]
        [Description("POST api/LienHe: null model must return BadRequest 400")]
        public void CreateLienHe_WithNullModel_ShouldReturnBadRequest()
        {
            // Arrange
            var controller = CreateControllerWithHttpConfig();

            // Act
            var result = controller.CreateLienHe(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult),
                "When model is null, must return BadRequestErrorMessageResult");

            var badRequest = (BadRequestErrorMessageResult)result;
            Assert.IsTrue(badRequest.Message.Contains("khong duoc de trong") ||
                         badRequest.Message.Contains("trong"),
                $"Message must contain appropriate error, actual: '{badRequest.Message}'");
        }

        [TestMethod]
        [TestCategory("LienHe_Validation")]
        [Description("POST api/LienHe: empty HOTEN must return BadRequest 400")]
        public void CreateLienHe_WithEmptyHoTen_ShouldReturnBadRequest()
        {
            // Arrange
            var controller = CreateControllerWithHttpConfig();
            var model = new LIENHE
            {
                HOTEN = "",    // <-- empty
                EMAIL = "test@email.com",
                DIENTHOAI = "0901234567",
                NOIDUNG = "Noi dung lien he test"
            };

            // Act
            var result = controller.CreateLienHe(model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult),
                "Empty HOTEN must return BadRequest");

            var badRequest = (BadRequestErrorMessageResult)result;
            Assert.IsTrue(badRequest.Message.ToLower().Contains("ho ten") ||
                         badRequest.Message.ToLower().Contains("ten") ||
                         badRequest.Message.ToLower().Contains("trong"),
                $"Message must mention HOTEN, actual: '{badRequest.Message}'");
        }

        [TestMethod]
        [TestCategory("LienHe_Validation")]
        [Description("POST api/LienHe: whitespace-only HOTEN must return BadRequest")]
        public void CreateLienHe_WithWhitespaceHoTen_ShouldReturnBadRequest()
        {
            // Arrange
            var controller = CreateControllerWithHttpConfig();
            var model = new LIENHE
            {
                HOTEN = "   ",   // <-- whitespace only
                EMAIL = "test@email.com",
                DIENTHOAI = "0901234567",
                NOIDUNG = "Noi dung test"
            };

            // Act
            var result = controller.CreateLienHe(model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult),
                "Whitespace-only HOTEN must return BadRequest (IsNullOrWhiteSpace check)");
        }

        // GROUP 2: LIENHE Model Validation Attribute Tests

        [TestMethod]
        [TestCategory("LienHe_Model")]
        [Description("LIENHE model: valid email must pass validation")]
        public void LienHeModel_WithValidEmail_ShouldPassValidation()
        {
            // Arrange
            var model = new LIENHE
            {
                HOTEN = "Nguyen Van A",
                EMAIL = "nguyenvana@gmail.com",
                DIENTHOAI = "0901234567",
                NOIDUNG = "Toi muon hoi ve san pham hoa hong"
            };

            // Act
            var validationResults = ValidationHelper.ValidateModel(model);

            // Assert
            Assert.AreEqual(0, validationResults.Count,
                $"Valid model must have no validation errors. Errors found: {string.Join(", ", validationResults.Select(v => v.ErrorMessage))}");
        }

        [TestMethod]
        [TestCategory("LienHe_Model")]
        [Description("LIENHE model: phone not starting with 0 must fail validation")]
        public void LienHeModel_WithInvalidPhone_NotStartingWith0_ShouldFailValidation()
        {
            // Arrange - phone starts with 1 (wrong, rule is ^0\d{9}$)
            var model = new LIENHE
            {
                HOTEN = "Tran Thi B",
                EMAIL = "tranthib@email.com",
                DIENTHOAI = "1234567890", // Invalid: does not start with 0
                NOIDUNG = "Noi dung test"
            };

            // Act
            var validationResults = ValidationHelper.ValidateModel(model);

            // Assert
            var phoneError = validationResults.FirstOrDefault(v =>
                v.MemberNames.Contains("DIENTHOAI") ||
                v.ErrorMessage.ToLower().Contains("dien thoai"));

            Assert.IsNotNull(phoneError,
                "Phone not starting with 0 must produce a validation error for DIENTHOAI");
        }

        [TestMethod]
        [TestCategory("LienHe_Model")]
        [Description("LIENHE model: phone less than 10 digits must fail validation")]
        public void LienHeModel_WithShortPhone_ShouldFailValidation()
        {
            // Arrange - phone has only 9 digits
            var model = new LIENHE
            {
                HOTEN = "Le Van C",
                EMAIL = "levanc@email.com",
                DIENTHOAI = "090123456",  // Invalid: only 9 digits, missing 1
                NOIDUNG = "Test noi dung"
            };

            // Act
            var validationResults = ValidationHelper.ValidateModel(model);

            // Assert
            Assert.IsTrue(validationResults.Any(),
                "Phone with 9 digits (missing 1) must produce a validation error");
        }

        [TestMethod]
        [TestCategory("LienHe_Model")]
        [Description("LIENHE model: NOIDUNG over 500 chars must fail StringLength validation")]
        public void LienHeModel_WithNoidungTooLong_ShouldFailValidation()
        {
            // Arrange - content 501 chars
            var model = new LIENHE
            {
                HOTEN = "Pham Thi D",
                EMAIL = "phamthid@email.com",
                DIENTHOAI = "0909876543",
                NOIDUNG = new string('X', 501) // Exceeds limit of 500 chars
            };

            // Act
            var validationResults = ValidationHelper.ValidateModel(model);

            // Assert
            var noidungError = validationResults.FirstOrDefault(v =>
                v.MemberNames.Contains("NOIDUNG") ||
                v.ErrorMessage.ToLower().Contains("noi dung"));

            Assert.IsNotNull(noidungError,
                "NOIDUNG exceeding 500 chars must produce a StringLength validation error");
        }

        [TestMethod]
        [TestCategory("LienHe_Model")]
        [Description("LIENHE model: HOTEN over 100 chars must fail StringLength validation")]
        public void LienHeModel_WithHotenTooLong_ShouldFailValidation()
        {
            // Arrange - name 101 chars
            var model = new LIENHE
            {
                HOTEN = new string('A', 101), // Exceeds limit of 100 chars
                EMAIL = "test@email.com",
                DIENTHOAI = "0912345678",
                NOIDUNG = "Noi dung binh thuong"
            };

            // Act
            var validationResults = ValidationHelper.ValidateModel(model);

            // Assert
            var hotenError = validationResults.FirstOrDefault(v =>
                v.MemberNames.Contains("HOTEN") ||
                v.ErrorMessage.ToLower().Contains("ten"));

            Assert.IsNotNull(hotenError,
                "HOTEN exceeding 100 chars must produce a StringLength validation error");
        }

        // GROUP 3: Stored Procedure Signature Tests

        [TestMethod]
        [TestCategory("LienHe_StoredProc")]
        [Description("Verify sp_ThemLienHe is defined with correct 5 parameters in DbContext")]
        public void DbContext_SpThemLienHe_ShouldHaveCorrectSignature()
        {
            // Arrange
            var contextType = typeof(QL_SHOPFLOWEREntities);
            var method = contextType.GetMethod("sp_ThemLienHe");

            // Assert - method must exist
            Assert.IsNotNull(method,
                "sp_ThemLienHe must be defined in QL_SHOPFLOWEREntities");

            // Assert - must have exactly 5 params: hoTen, email, dienThoai, noiDung, maLH
            var parameters = method.GetParameters();
            Assert.AreEqual(5, parameters.Length,
                $"sp_ThemLienHe must have 5 parameters, actual: {parameters.Length}");

            // Verify parameter names
            var paramNames = parameters.Select(p => p.Name.ToLower()).ToList();
            Assert.IsTrue(paramNames.Contains("hoten"), "Must have parameter hoTen");
            Assert.IsTrue(paramNames.Contains("email"), "Must have parameter email");
            Assert.IsTrue(paramNames.Contains("dienthoai"), "Must have parameter dienThoai");
            Assert.IsTrue(paramNames.Contains("noidung"), "Must have parameter noiDung");
            Assert.IsTrue(paramNames.Contains("malh"), "Must have parameter maLH (output)");
        }

        [TestMethod]
        [TestCategory("LienHe_Model")]
        [Description("Verify LIENHE model has all 5 required properties per DB design")]
        public void LienHeModel_ShouldHaveRequiredProperties()
        {
            // Arrange
            var modelType = typeof(LIENHE);

            // Assert - check required properties
            var properties = modelType.GetProperties().Select(p => p.Name).ToList();

            Assert.IsTrue(properties.Contains("MALH"), "LIENHE must have property MALH");
            Assert.IsTrue(properties.Contains("HOTEN"), "LIENHE must have property HOTEN");
            Assert.IsTrue(properties.Contains("EMAIL"), "LIENHE must have property EMAIL");
            Assert.IsTrue(properties.Contains("DIENTHOAI"), "LIENHE must have property DIENTHOAI");
            Assert.IsTrue(properties.Contains("NOIDUNG"), "LIENHE must have property NOIDUNG");
        }

        // Helper Methods

        private LienHeController CreateControllerWithHttpConfig()
        {
            var controller = new LienHeController();
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage();
            request.SetConfiguration(config);
            controller.Request = request;
            controller.Configuration = config;
            return controller;
        }
    }

    internal static class ValidationHelper
    {
        public static IList<System.ComponentModel.DataAnnotations.ValidationResult> ValidateModel(object model)
        {
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var context = new System.ComponentModel.DataAnnotations.ValidationContext(model, null, null);
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(model, context, results, true);
            return results;
        }
    }
}
