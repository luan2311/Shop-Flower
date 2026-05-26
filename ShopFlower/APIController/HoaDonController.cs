using ShopFlower.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ShopFlower.APIController
{
    public class HoaDonController : ApiController
    {
        private readonly QL_SHOPFLOWEREntities db = new QL_SHOPFLOWEREntities();

        public class OrderItemModel
        {
            public string MaSP { get; set; }
            public int SoLuong { get; set; }
            public decimal DonGia { get; set; }
        }

        public class PlaceOrderModel
        {
            public int MaTK { get; set; }
            public string TenNguoiNhan { get; set; }
            public string Email { get; set; }
            public string SoDienThoai { get; set; }
            public string DiaChiGiaoHang { get; set; }
            public string PhuongXa { get; set; }
            public string QuanHuyen { get; set; }
            public string TinhThanh { get; set; }
            public string GhiChu { get; set; }
            public string PhuongThucThanhToan { get; set; }
            public List<OrderItemModel> Items { get; set; }
        }

        public class StatusUpdateModel
        {
            public string Status { get; set; }
        }

        // GET: api/HoaDon (Quản lý toàn bộ hóa đơn cho Admin)
        [HttpGet]
        public IHttpActionResult GetAllOrders()
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;

                var orders = db.HOADONs
                    .OrderByDescending(h => h.NgayDat)
                    .ToList();

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Lỗi khi lấy danh sách đơn hàng: {ex.Message}"));
            }
        }

        // GET: api/HoaDon/MyOrders?userId=5 (Lịch sử đơn hàng của người dùng)
        [HttpGet]
        [Route("api/HoaDon/MyOrders")]
        public IHttpActionResult GetMyOrders(int userId)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;

                var orders = db.HOADONs
                    .Where(h => h.MaTK == userId)
                    .OrderByDescending(h => h.NgayDat)
                    .ToList();

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Lỗi khi lấy lịch sử đơn hàng: {ex.Message}"));
            }
        }

        // GET: api/HoaDon/5 (Chi tiết đơn hàng kèm các mặt hàng)
        [HttpGet]
        public IHttpActionResult GetOrderDetails(int id)
        {
            try
            {
                var order = db.HOADONs.FirstOrDefault(h => h.MaHD == id);
                if (order == null)
                {
                    return NotFound();
                }

                // Map sang DTO sạch để tránh circular reference của Entity Framework
                var result = new
                {
                    MaHD = order.MaHD,
                    MaTK = order.MaTK,
                    NgayDat = order.NgayDat,
                    TongTien = order.TongTien,
                    DiaChiNhan = order.DiaChiNhan,
                    SDTNhan = order.SDTNhan,
                    TenNguoiNhan = order.TenNguoiNhan,
                    Email = order.Email,
                    GhiChu = order.GhiChu,
                    TrangThai = order.TrangThai,
                    PhuongThucThanhToan = order.PhuongThucThanhToan,
                    Details = order.CTHDs.Select(ct => new
                    {
                        MaSP = ct.MaSP,
                        TenSP = ct.SANPHAM?.TenSP,
                        AnhSP = ct.SANPHAM?.AnhSP,
                        SoLuong = ct.SoLuong,
                        DonGia = ct.DonGia
                    }).ToList()
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Lỗi khi lấy chi tiết đơn hàng: {ex.Message}"));
            }
        }

        // POST: api/HoaDon (Đặt hàng trực tiếp qua API)
        [HttpPost]
        public IHttpActionResult PlaceOrder([FromBody] PlaceOrderModel model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest("Dữ liệu đặt hàng không được để trống");
                }

                if (model.Items == null || !model.Items.Any())
                {
                    return BadRequest("Giỏ hàng của bạn đang trống");
                }

                if (string.IsNullOrWhiteSpace(model.TenNguoiNhan) || string.IsNullOrWhiteSpace(model.SoDienThoai) || string.IsNullOrWhiteSpace(model.DiaChiGiaoHang))
                {
                    return BadRequest("Vui lòng nhập đầy đủ thông tin người nhận và địa chỉ");
                }

                // Kiểm tra tài khoản tồn tại
                var userExists = db.TAIKHOANs.Any(t => t.MaTK == model.MaTK);
                if (!userExists)
                {
                    return BadRequest("Mã tài khoản đặt hàng không hợp lệ");
                }

                // Ghép địa chỉ đầy đủ
                var diaChiDayDu = string.Format("{0}, {1}, {2}, {3}",
                    model.DiaChiGiaoHang,
                    model.PhuongXa ?? "",
                    model.QuanHuyen ?? "",
                    model.TinhThanh ?? "");

                // Tính tổng tiền từ danh sách items
                decimal tongTien = model.Items.Sum(item => item.SoLuong * item.DonGia);

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        // 1. Tạo hóa đơn
                        var hoadon = new HOADON
                        {
                            MaTK = model.MaTK,
                            NgayDat = DateTime.Now,
                            TongTien = tongTien,
                            DiaChiNhan = diaChiDayDu,
                            SDTNhan = model.SoDienThoai,
                            TenNguoiNhan = model.TenNguoiNhan,
                            Email = model.Email,
                            GhiChu = model.GhiChu,
                            TrangThai = "Pending",
                            PhuongThucThanhToan = model.PhuongThucThanhToan ?? "COD"
                        };

                        db.HOADONs.Add(hoadon);
                        db.SaveChanges(); // Lưu để có MaHD

                        // 2. Tạo chi tiết hóa đơn
                        foreach (var item in model.Items)
                        {
                            // Kiểm tra sản phẩm hợp lệ
                            var product = db.SANPHAMs.FirstOrDefault(sp => sp.MaSP == item.MaSP);
                            if (product == null)
                            {
                                throw new Exception($"Mã sản phẩm {item.MaSP} không tồn tại");
                            }

                            var cthd = new CTHD
                            {
                                MaHD = hoadon.MaHD,
                                MaSP = item.MaSP,
                                SoLuong = item.SoLuong,
                                DonGia = item.DonGia
                            };
                            db.CTHDs.Add(cthd);
                        }

                        db.SaveChanges();
                        transaction.Commit();

                        return Ok(new
                        {
                            success = true,
                            message = "Đặt hàng thành công",
                            MaHD = hoadon.MaHD,
                            TongTien = hoadon.TongTien
                        });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return BadRequest($"Đặt hàng thất bại: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Lỗi hệ thống khi đặt hàng: {ex.Message}"));
            }
        }

        // PUT: api/HoaDon/5/status (Duyệt/Cập nhật trạng thái hóa đơn)
        [HttpPut]
        [Route("api/HoaDon/{id}/status")]
        public IHttpActionResult UpdateStatus(int id, [FromBody] StatusUpdateModel model)
        {
            try
            {
                if (model == null || string.IsNullOrWhiteSpace(model.Status))
                {
                    return BadRequest("Vui lòng cung cấp trạng thái mới");
                }

                var order = db.HOADONs.FirstOrDefault(h => h.MaHD == id);
                if (order == null)
                {
                    return NotFound();
                }

                order.TrangThai = model.Status;
                db.SaveChanges();

                return Ok(new
                {
                    success = true,
                    message = $"Cập nhật trạng thái đơn hàng #{id} thành '{model.Status}' thành công",
                    MaHD = order.MaHD,
                    TrangThai = order.TrangThai
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Lỗi khi cập nhật trạng thái đơn hàng: {ex.Message}"));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
