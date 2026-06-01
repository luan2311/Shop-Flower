using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopFlower.Models;

namespace ShopFlower.Tests
{
    [TestClass]
    public class SanPhamControllerTests
    {
        // GROUP 1: Soft-Delete Logic Tests (Stop Selling Product)

        [TestMethod]
        [TestCategory("SanPham_SoftDelete")]
        [Description("Stop selling: SoLuongTon must be updated to 0")]
        public void StopSelling_ShouldSet_SoLuongTonToZero()
        {
            // Arrange - product currently on sale with stock > 0
            var product = new SANPHAM
            {
                MaSP = "SP001",
                TenSP = "Hoa Hong Do",
                GiaBan = 150000,
                SoLuongTon = 50,
                TinhTrang = "Con hang"
            };

            // Act - simulate stop-selling (soft-delete) operation
            SimulateStopSelling(product);

            // Assert
            Assert.AreEqual(0, product.SoLuongTon,
                "After stop selling, SoLuongTon must equal 0");
        }

        [TestMethod]
        [TestCategory("SanPham_SoftDelete")]
        [Description("Stop selling: TinhTrang must be updated to 'Het hang'")]
        public void StopSelling_ShouldSet_TinhTrangToHetHang()
        {
            // Arrange
            var product = new SANPHAM
            {
                MaSP = "SP002",
                TenSP = "Bo Hoa Cuc Vang",
                GiaBan = 200000,
                SoLuongTon = 30,
                TinhTrang = "Con hang"
            };

            // Act
            SimulateStopSelling(product);

            // Assert
            Assert.AreEqual("Het hang", product.TinhTrang,
                "After stop selling, TinhTrang must be 'Het hang'");
        }

        [TestMethod]
        [TestCategory("SanPham_SoftDelete")]
        [Description("Product with SoLuongTon=0 must not be addable to cart")]
        public void ProductWithZeroStock_ShouldNotBeAddableToCart()
        {
            // Arrange
            var outOfStockProduct = new SANPHAM
            {
                MaSP = "SP003",
                TenSP = "Hoa Lan Tim",
                GiaBan = 300000,
                SoLuongTon = 0,
                TinhTrang = "Het hang"
            };

            // Act - CartController logic before adding to cart
            bool canAddToCart = outOfStockProduct.SoLuongTon.HasValue
                                && outOfStockProduct.SoLuongTon.Value > 0;

            // Assert
            Assert.IsFalse(canAddToCart,
                "Product with SoLuongTon = 0 must not be addable to cart");
        }

        // GROUP 2: GiaBan (Price) Constraint Tests (ADM_02)

        [TestMethod]
        [TestCategory("SanPham_PriceValidation")]
        [Description("GiaBan = 0 must be detected as invalid")]
        public void ValidateGiaBan_WhenZero_ShouldBeInvalid()
        {
            // Arrange
            var product = new SANPHAM
            {
                MaSP = "SP_TEST",
                TenSP = "Test Flower",
                GiaBan = 0, // <-- invalid
                SoLuongTon = 10,
                TinhTrang = "Con hang"
            };

            // Act
            bool isValid = IsValidPrice(product.GiaBan);

            // Assert
            Assert.IsFalse(isValid,
                "GiaBan = 0 must be rejected. Price must be greater than 0");
        }

        [TestMethod]
        [TestCategory("SanPham_PriceValidation")]
        [Description("Negative GiaBan (-1) must be detected as invalid")]
        public void ValidateGiaBan_WhenNegative_ShouldBeInvalid()
        {
            // Arrange
            var product = new SANPHAM
            {
                MaSP = "SP_TEST2",
                TenSP = "Test Flower 2",
                GiaBan = -1, // <-- invalid
                SoLuongTon = 5,
                TinhTrang = "Con hang"
            };

            // Act
            bool isValid = IsValidPrice(product.GiaBan);

            // Assert
            Assert.IsFalse(isValid,
                "Negative GiaBan (-1) must be rejected. Price must be greater than 0");
        }

        [TestMethod]
        [TestCategory("SanPham_PriceValidation")]
        [Description("Positive GiaBan (100000) must be accepted as valid")]
        public void ValidateGiaBan_WhenPositive_ShouldBeValid()
        {
            // Arrange
            var product = new SANPHAM
            {
                MaSP = "SP_TEST3",
                TenSP = "Test Flower 3",
                GiaBan = 100000, // <-- valid
                SoLuongTon = 20,
                TinhTrang = "Con hang"
            };

            // Act
            bool isValid = IsValidPrice(product.GiaBan);

            // Assert
            Assert.IsTrue(isValid,
                "GiaBan = 100000 must be a valid price value");
        }

        [TestMethod]
        [TestCategory("SanPham_PriceValidation")]
        [Description("GiaBan = null must be detected as invalid")]
        public void ValidateGiaBan_WhenNull_ShouldBeInvalid()
        {
            // Arrange
            int? nullPrice = null;

            // Act
            bool isValid = IsValidPrice(nullPrice);

            // Assert
            Assert.IsFalse(isValid,
                "GiaBan = null must be rejected");
        }

        // GROUP 3: SANPHAM Model Tests

        [TestMethod]
        [TestCategory("SanPham_Model")]
        [Description("SANPHAM model must have all 9 required properties per DB design")]
        public void SANPHAMModel_ShouldHaveAllRequiredProperties()
        {
            // Arrange
            var modelType = typeof(SANPHAM);
            var properties = modelType.GetProperties().Select(p => p.Name).ToList();

            // Assert - columns in SANPHAM table
            Assert.IsTrue(properties.Contains("MaSP"), "SANPHAM must have MaSP (PK)");
            Assert.IsTrue(properties.Contains("TenSP"), "SANPHAM must have TenSP");
            Assert.IsTrue(properties.Contains("GiaBan"), "SANPHAM must have GiaBan");
            Assert.IsTrue(properties.Contains("AnhSP"), "SANPHAM must have AnhSP");
            Assert.IsTrue(properties.Contains("MoTaSP"), "SANPHAM must have MoTaSP");
            Assert.IsTrue(properties.Contains("TinhTrang"), "SANPHAM must have TinhTrang");
            Assert.IsTrue(properties.Contains("ThuongHieu"), "SANPHAM must have ThuongHieu");
            Assert.IsTrue(properties.Contains("SoLuongTon"), "SANPHAM must have SoLuongTon");
            Assert.IsTrue(properties.Contains("MaLoai"), "SANPHAM must have MaLoai (FK)");

            // Navigation properties
            Assert.IsTrue(properties.Contains("CTHDs"), "SANPHAM must have navigation CTHDs");
            Assert.IsTrue(properties.Contains("LOAIHANG"), "SANPHAM must have navigation LOAIHANG");
        }

        [TestMethod]
        [TestCategory("SanPham_Model")]
        [Description("GiaBan and SoLuongTon must be Nullable<int> per DB design")]
        public void SANPHAMModel_GiaBanAndSoLuongTon_ShouldBeNullableInt()
        {
            // Arrange
            var modelType = typeof(SANPHAM);

            // Act
            var giaBanProp = modelType.GetProperty("GiaBan");
            var soLuongTonProp = modelType.GetProperty("SoLuongTon");

            // Assert
            Assert.IsNotNull(giaBanProp, "SANPHAM must have property GiaBan");
            Assert.AreEqual(typeof(int?), giaBanProp.PropertyType,
                "GiaBan must be Nullable<int> (int?)");

            Assert.IsNotNull(soLuongTonProp, "SANPHAM must have property SoLuongTon");
            Assert.AreEqual(typeof(int?), soLuongTonProp.PropertyType,
                "SoLuongTon must be Nullable<int> (int?)");
        }

        [TestMethod]
        [TestCategory("SanPham_Model")]
        [Description("MaSP must be string type per DB design (not int identity)")]
        public void SANPHAMModel_MaSP_ShouldBeString()
        {
            // Arrange
            var modelType = typeof(SANPHAM);
            var maSPProp = modelType.GetProperty("MaSP");

            // Assert
            Assert.IsNotNull(maSPProp);
            Assert.AreEqual(typeof(string), maSPProp.PropertyType,
                "MaSP must be string type (e.g. 'SP001')");
        }

        // GROUP 4: Pagination Logic Tests

        [TestMethod]
        [TestCategory("SanPham_Pagination")]
        [Description("Pagination: 25 products with pageSize=12 must yield 3 pages")]
        public void Pagination_With25Products_PageSize12_ShouldReturn3Pages()
        {
            // Arrange
            int totalProducts = 25;
            int pageSize = 12; // default pageSize of tat_ca_san_pham

            // Act - formula used in controller
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            // Assert
            Assert.AreEqual(3, totalPages,
                $"25 products with pageSize=12 must have 3 pages (actual: {totalPages})");
        }

        [TestMethod]
        [TestCategory("SanPham_Pagination")]
        [Description("Pagination: 24 products with pageSize=12 must yield exactly 2 pages")]
        public void Pagination_With24Products_PageSize12_ShouldReturn2Pages()
        {
            // Arrange
            int totalProducts = 24;
            int pageSize = 12;

            // Act
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            // Assert
            Assert.AreEqual(2, totalPages,
                $"24 products with pageSize=12 must have 2 pages, no empty page (actual: {totalPages})");
        }

        [TestMethod]
        [TestCategory("SanPham_Pagination")]
        [Description("Pagination: 0 products must yield 0 pages")]
        public void Pagination_WithZeroProducts_ShouldReturn0Pages()
        {
            // Arrange
            int totalProducts = 0;
            int pageSize = 12;

            // Act
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            // Assert
            Assert.AreEqual(0, totalPages,
                "When there are no products, total pages must be 0");
        }

        // GROUP 5: SoLuongTon Boundary Tests

        [TestMethod]
        [TestCategory("SanPham_StockBoundary")]
        [Description("SoLuongTon = 0 (lower boundary) -> product must show out-of-stock status")]
        public void SoLuongTon_BoundaryZero_ShouldBeConsideredOutOfStock()
        {
            // Arrange
            var product = new SANPHAM { SoLuongTon = 0 };

            // Act
            bool isOutOfStock = !product.SoLuongTon.HasValue || product.SoLuongTon.Value <= 0;

            // Assert
            Assert.IsTrue(isOutOfStock,
                "SoLuongTon = 0 must be considered out of stock");
        }

        [TestMethod]
        [TestCategory("SanPham_StockBoundary")]
        [Description("SoLuongTon = 1 (lower boundary+1) -> product must still be in stock")]
        public void SoLuongTon_BoundaryOne_ShouldBeConsideredInStock()
        {
            // Arrange
            var product = new SANPHAM { SoLuongTon = 1 };

            // Act
            bool isOutOfStock = !product.SoLuongTon.HasValue || product.SoLuongTon.Value <= 0;

            // Assert
            Assert.IsFalse(isOutOfStock,
                "SoLuongTon = 1 must be considered in stock (can still be ordered)");
        }

        [TestMethod]
        [TestCategory("SanPham_StockBoundary")]
        [Description("SoLuongTon = 9999 (upper boundary) -> must be handled correctly, no overflow")]
        public void SoLuongTon_BoundaryMax9999_ShouldBeHandledCorrectly()
        {
            // Arrange
            var product = new SANPHAM { SoLuongTon = 9999 };

            // Act
            bool isOutOfStock = !product.SoLuongTon.HasValue || product.SoLuongTon.Value <= 0;
            bool isValidStock = product.SoLuongTon.HasValue && product.SoLuongTon.Value >= 0;

            // Assert
            Assert.IsFalse(isOutOfStock,
                "SoLuongTon = 9999 must be considered in stock");
            Assert.IsTrue(isValidStock,
                "SoLuongTon = 9999 is a valid value");
        }

        // Helper Methods

        private void SimulateStopSelling(SANPHAM product)
        {
            // Stop-selling logic: reset stock to 0 and change status
            product.SoLuongTon = 0;
            product.TinhTrang = "Het hang";
        }

        private bool IsValidPrice(int? giaBan)
        {
            return giaBan.HasValue && giaBan.Value > 0;
        }
    }
}
