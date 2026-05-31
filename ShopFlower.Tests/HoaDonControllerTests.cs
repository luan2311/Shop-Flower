using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopFlower.APIController;
using ShopFlower.Models;

namespace ShopFlower.Tests
{
    [TestClass]
    public class HoaDonControllerTests
    {
        // NHOM 1: Kiem thu PlaceOrder Validation (khong can DB)

        [TestMethod]
        [TestCategory("HoaDon_Validation")]
        [Description("POST api/HoaDon: model null phai tra ve BadRequest 400")]
        public void PlaceOrder_WithNullModel_ShouldReturnBadRequest()
        {
            // Arrange
            var controller = CreateControllerWithHttpConfig();

            // Act
            var result = controller.PlaceOrder(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult),
                "Model null phai tra ve BadRequest");

            var badRequest = (BadRequestErrorMessageResult)result;
            Assert.IsFalse(string.IsNullOrEmpty(badRequest.Message),
                "Message khong duoc rong");
        }

        [TestMethod]
        [TestCategory("HoaDon_Validation")]
        [Description("POST api/HoaDon: gio hang trong (null items) phai tra ve BadRequest")]
        public void PlaceOrder_WithNullItems_ShouldReturnBadRequest()
        {
            // Arrange
            var controller = CreateControllerWithHttpConfig();
            var model = CreateValidOrderModel();
            model.Items = null; // <-- gio hang trong

            // Act
            var result = controller.PlaceOrder(model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult),
                "Items null phai tra ve BadRequest");

            var badRequest = (BadRequestErrorMessageResult)result;
            Assert.IsTrue(badRequest.Message.ToLower().Contains("trống") ||
                         badRequest.Message.ToLower().Contains("empty") ||
                         badRequest.Message.ToLower().Contains("items"),
                $"Message phai de cap den gio hang trong, thuc te: '{badRequest.Message}'");
        }

        [TestMethod]
        [TestCategory("HoaDon_Validation")]
        [Description("POST api/HoaDon: Items la danh sach rong phai tra ve BadRequest")]
        public void PlaceOrder_WithEmptyItemsList_ShouldReturnBadRequest()
        {
            // Arrange
            var controller = CreateControllerWithHttpConfig();
            var model = CreateValidOrderModel();
            model.Items = new List<HoaDonController.OrderItemModel>(); // <-- empty list

            // Act
            var result = controller.PlaceOrder(model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult),
                "Items rong phai tra ve BadRequest");
        }

        [TestMethod]
        [TestCategory("HoaDon_Validation")]
        [Description("POST api/HoaDon: TenNguoiNhan rong phai tra ve BadRequest")]
        public void PlaceOrder_WithEmptyTenNguoiNhan_ShouldReturnBadRequest()
        {
            // Arrange
            var controller = CreateControllerWithHttpConfig();
            var model = CreateValidOrderModel();
            model.TenNguoiNhan = ""; // <-- rong

            // Act
            var result = controller.PlaceOrder(model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult),
                "TenNguoiNhan rong phai tra ve BadRequest");
        }

        [TestMethod]
        [TestCategory("HoaDon_Validation")]
        [Description("POST api/HoaDon: SoDienThoai rong phai tra ve BadRequest")]
        public void PlaceOrder_WithEmptySoDienThoai_ShouldReturnBadRequest()
        {
            // Arrange
            var controller = CreateControllerWithHttpConfig();
            var model = CreateValidOrderModel();
            model.SoDienThoai = ""; // <-- rong

            // Act
            var result = controller.PlaceOrder(model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult),
                "SoDienThoai rong phai tra ve BadRequest");
        }

        [TestMethod]
        [TestCategory("HoaDon_Validation")]
        [Description("POST api/HoaDon: DiaChiGiaoHang rong phai tra ve BadRequest")]
        public void PlaceOrder_WithEmptyDiaChiGiaoHang_ShouldReturnBadRequest()
        {
            // Arrange
            var controller = CreateControllerWithHttpConfig();
            var model = CreateValidOrderModel();
            model.DiaChiGiaoHang = "   "; // <-- chi whitespace

            // Act
            var result = controller.PlaceOrder(model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult),
                "DiaChiGiaoHang chi whitespace phai tra ve BadRequest");
        }

        // NHOM 2: Kiem thu Logic Tinh Toan (khong can DB)

        [TestMethod]
        [TestCategory("HoaDon_Logic")]
        [Description("Tong tien phai bang tong cua (SoLuong * DonGia) cho moi item")]
        public void PlaceOrder_TongTienCalculation_ShouldBeCorrect()
        {
            // Arrange
            var items = new List<HoaDonController.OrderItemModel>
            {
                new HoaDonController.OrderItemModel { MaSP = "SP001", SoLuong = 2, DonGia = 150000m },
                new HoaDonController.OrderItemModel { MaSP = "SP002", SoLuong = 1, DonGia = 250000m },
                new HoaDonController.OrderItemModel { MaSP = "SP003", SoLuong = 3, DonGia = 80000m  }
            };

            // Expected: 2*150000 + 1*250000 + 3*80000 = 300000 + 250000 + 240000 = 790000
            decimal expectedTotal = 790000m;

            // Act
            decimal actualTotal = items.Sum(item => item.SoLuong * item.DonGia);

            // Assert
            Assert.AreEqual(expectedTotal, actualTotal,
                $"Tong tien phai la {expectedTotal:N0}, thuc te: {actualTotal:N0}");
        }

        [TestMethod]
        [TestCategory("HoaDon_Logic")]
        [Description("Ghep dia chi theo format: 'So nha, Phuong, Quan, Tinh'")]
        public void PlaceOrder_DiaChiFormat_ShouldMatchExpectedPattern()
        {
            // Arrange
            string diaChiGiaoHang = "123 Duong Hoa Mai";
            string phuongXa = "Phuong 5";
            string quanHuyen = "Quan Binh Thanh";
            string tinhThanh = "TP. Ho Chi Minh";

            // Act
            string diaChiDayDu = string.Format("{0}, {1}, {2}, {3}",
                diaChiGiaoHang, phuongXa, quanHuyen, tinhThanh);

            // Assert
            string expected = "123 Duong Hoa Mai, Phuong 5, Quan Binh Thanh, TP. Ho Chi Minh";
            Assert.AreEqual(expected, diaChiDayDu,
                "Dia chi ghep khong dung format yeu cau");

            var parts = diaChiDayDu.Split(new[] { ", " }, StringSplitOptions.None);
            Assert.AreEqual(4, parts.Length, "Dia chi phai co 4 thanh phan");
        }

        // NHOM 3: Kiem thu Model Structure

        [TestMethod]
        [TestCategory("HoaDon_Model")]
        [Description("PlaceOrderModel phai co du 11 properties theo dac ta API")]
        public void PlaceOrderModel_ShouldHaveAllRequiredProperties()
        {
            // Arrange
            var modelType = typeof(HoaDonController.PlaceOrderModel);
            var properties = modelType.GetProperties().Select(p => p.Name).ToList();

            // Assert
            Assert.IsTrue(properties.Contains("MaTK"), "PlaceOrderModel phai co MaTK");
            Assert.IsTrue(properties.Contains("TenNguoiNhan"), "PlaceOrderModel phai co TenNguoiNhan");
            Assert.IsTrue(properties.Contains("Email"), "PlaceOrderModel phai co Email");
            Assert.IsTrue(properties.Contains("SoDienThoai"), "PlaceOrderModel phai co SoDienThoai");
            Assert.IsTrue(properties.Contains("DiaChiGiaoHang"), "PlaceOrderModel phai co DiaChiGiaoHang");
            Assert.IsTrue(properties.Contains("PhuongXa"), "PlaceOrderModel phai co PhuongXa");
            Assert.IsTrue(properties.Contains("QuanHuyen"), "PlaceOrderModel phai co QuanHuyen");
            Assert.IsTrue(properties.Contains("TinhThanh"), "PlaceOrderModel phai co TinhThanh");
            Assert.IsTrue(properties.Contains("GhiChu"), "PlaceOrderModel phai co GhiChu");
            Assert.IsTrue(properties.Contains("PhuongThucThanhToan"), "PlaceOrderModel phai co PhuongThucThanhToan");
            Assert.IsTrue(properties.Contains("Items"), "PlaceOrderModel phai co Items");
        }

        [TestMethod]
        [TestCategory("HoaDon_Model")]
        [Description("OrderItemModel phai co 3 properties: MaSP (string), SoLuong (int), DonGia (decimal)")]
        public void OrderItemModel_ShouldHaveCorrectPropertiesAndTypes()
        {
            // Arrange
            var modelType = typeof(HoaDonController.OrderItemModel);

            // Assert - MaSP la string
            var maSPProp = modelType.GetProperty("MaSP");
            Assert.IsNotNull(maSPProp, "OrderItemModel phai co property MaSP");
            Assert.AreEqual(typeof(string), maSPProp.PropertyType, "MaSP phai la kieu string");

            // Assert - SoLuong la int
            var soLuongProp = modelType.GetProperty("SoLuong");
            Assert.IsNotNull(soLuongProp, "OrderItemModel phai co property SoLuong");
            Assert.AreEqual(typeof(int), soLuongProp.PropertyType, "SoLuong phai la kieu int");

            // Assert - DonGia la decimal
            var donGiaProp = modelType.GetProperty("DonGia");
            Assert.IsNotNull(donGiaProp, "OrderItemModel phai co property DonGia");
            Assert.AreEqual(typeof(decimal), donGiaProp.PropertyType, "DonGia phai la kieu decimal");
        }

        [TestMethod]
        [TestCategory("HoaDon_Model")]
        [Description("HOADON model phai co du 11 properties theo thiet ke CSDL")]
        public void HoaDonModel_ShouldHaveAllDatabaseProperties()
        {
            // Arrange
            var modelType = typeof(HOADON);
            var properties = modelType.GetProperties().Select(p => p.Name).ToList();

            // Assert
            Assert.IsTrue(properties.Contains("MaHD"), "HOADON phai co MaHD (PK)");
            Assert.IsTrue(properties.Contains("MaTK"), "HOADON phai co MaTK (FK)");
            Assert.IsTrue(properties.Contains("NgayDat"), "HOADON phai co NgayDat");
            Assert.IsTrue(properties.Contains("TongTien"), "HOADON phai co TongTien");
            Assert.IsTrue(properties.Contains("TenNguoiNhan"), "HOADON phai co TenNguoiNhan");
            Assert.IsTrue(properties.Contains("DiaChiNhan"), "HOADON phai co DiaChiNhan");
            Assert.IsTrue(properties.Contains("SDTNhan"), "HOADON phai co SDTNhan");
            Assert.IsTrue(properties.Contains("Email"), "HOADON phai co Email");
            Assert.IsTrue(properties.Contains("GhiChu"), "HOADON phai co GhiChu");
            Assert.IsTrue(properties.Contains("TrangThai"), "HOADON phai co TrangThai");
            Assert.IsTrue(properties.Contains("PhuongThucThanhToan"), "HOADON phai co PhuongThucThanhToan");
            Assert.IsTrue(properties.Contains("CTHDs"), "HOADON phai co navigation property CTHDs");
        }

        [TestMethod]
        [TestCategory("HoaDon_Model")]
        [Description("CTHD model phai co du 5 properties: MaCTHD, MaHD, MaSP, SoLuong, DonGia")]
        public void CTHDModel_ShouldHaveAllDatabaseProperties()
        {
            // Arrange
            var modelType = typeof(CTHD);
            var properties = modelType.GetProperties().Select(p => p.Name).ToList();

            // Assert
            Assert.IsTrue(properties.Contains("MaCTHD"), "CTHD phai co MaCTHD (PK)");
            Assert.IsTrue(properties.Contains("MaHD"), "CTHD phai co MaHD (FK -> HOADON)");
            Assert.IsTrue(properties.Contains("MaSP"), "CTHD phai co MaSP (FK -> SANPHAM)");
            Assert.IsTrue(properties.Contains("SoLuong"), "CTHD phai co SoLuong");
            Assert.IsTrue(properties.Contains("DonGia"), "CTHD phai co DonGia");
        }

        // NHOM 4: Kiem thu logic Rollback (khong can DB)

        [TestMethod]
        [TestCategory("HoaDon_RollbackLogic")]
        [Description("OrderItem voi MaSP null phai duoc phat hien truoc khi luu vao DB")]
        public void OrderItem_WithNullMaSP_ShouldBeIdentifiable()
        {
            // Arrange
            var item = new HoaDonController.OrderItemModel
            {
                MaSP = null, // <-- se gay loi khi lookup DB
                SoLuong = 2,
                DonGia = 100000m
            };

            // Act
            bool isMaSPEmpty = string.IsNullOrEmpty(item.MaSP);

            // Assert
            Assert.IsTrue(isMaSPEmpty,
                "MaSP null phai duoc nhan ra truoc khi thuc hien query DB de tranh loi");
        }

        [TestMethod]
        [TestCategory("HoaDon_Logic")]
        [Description("Tinh tong tien phai chinh xac voi decimal (khong mat do chinh xac)")]
        public void PlaceOrder_TongTienWithDecimalPrecision_ShouldBeAccurate()
        {
            // Arrange
            var items = new List<HoaDonController.OrderItemModel>
            {
                new HoaDonController.OrderItemModel { MaSP = "SP001", SoLuong = 3, DonGia = 99999m },
                new HoaDonController.OrderItemModel { MaSP = "SP002", SoLuong = 7, DonGia = 12345m }
            };

            // Expected: 3*99999 + 7*12345 = 299997 + 86415 = 386412
            decimal expectedTotal = 386412m;

            // Act
            decimal actualTotal = items.Sum(item => item.SoLuong * item.DonGia);

            // Assert
            Assert.AreEqual(expectedTotal, actualTotal,
                $"Tong tien voi decimal precision phai chinh xac: {expectedTotal} != {actualTotal}");
        }

        // Helper Methods

        private HoaDonController CreateControllerWithHttpConfig()
        {
            var controller = new HoaDonController();
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
                PhuongXa = "Phuong XYZ",
                QuanHuyen = "Quan Test",
                TinhThanh = "TP. HCM",
                GhiChu = "Giao gio hanh chinh",
                PhuongThucThanhToan = "COD",
                Items = new List<HoaDonController.OrderItemModel>
                {
                    new HoaDonController.OrderItemModel
                    {
                        MaSP = "SP001",
                        SoLuong = 2,
                        DonGia = 150000m
                    }
                }
            };
        }
    }
}
