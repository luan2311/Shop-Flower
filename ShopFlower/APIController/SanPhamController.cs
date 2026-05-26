using ShopFlower.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ShopFlower.APIController
{
    public class SanPhamController : ApiController
    {
        // GET: api/SanPham
        [HttpGet]
        public IHttpActionResult GetAllSanPham()
        {
            try
            {
                using (var db = new QL_SHOPFLOWEREntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;

                    var sanPhams = db.SANPHAMs.ToList();

                    // Xóa navigation properties để tránh circular reference
                    foreach (var sp in sanPhams)
                    {
                        sp.LOAIHANG = null;
                        sp.CTHDs = null;
                    }

                    return Ok(sanPhams);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Lỗi: {ex.Message}"));
            }
        }

        // GET: api/SanPham/SP001
        [HttpGet]
        public IHttpActionResult GetSanPhamById(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest("ID không được để trống");
                }

                using (var db = new QL_SHOPFLOWEREntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;

                    var sanPham = db.SANPHAMs.FirstOrDefault(sp => sp.MaSP == id);

                    if (sanPham == null)
                    {
                        return NotFound();
                    }

                    // Xóa navigation properties
                    sanPham.LOAIHANG = null;
                    sanPham.CTHDs = null;

                    return Ok(sanPham);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Lỗi: {ex.Message}"));
            }
        }

        // POST: api/SanPham
        [HttpPost]
        public IHttpActionResult CreateSanPham([FromBody] SANPHAM sanPham)
        {
            try
            {
                if (sanPham == null)
                {
                    return BadRequest("Dữ liệu sản phẩm không được để trống");
                }

                if (string.IsNullOrWhiteSpace(sanPham.TenSP))
                {
                    return BadRequest("Tên sản phẩm không được để trống");
                }

                using (var db = new QL_SHOPFLOWEREntities())
                {
                    // Kiểm tra MaLoai có tồn tại
                    if (!string.IsNullOrEmpty(sanPham.MaLoai))
                    {
                        var loaiExists = db.LOAIHANGs.Any(lh => lh.MaLoai == sanPham.MaLoai);
                        if (!loaiExists)
                        {
                            return BadRequest("Mã loại sản phẩm không tồn tại");
                        }
                    }

                    // Tự động sinh mã sản phẩm mới
                    sanPham.MaSP = GenerateMaSanPham(db);

                    // Thiết lập giá trị mặc định nếu cần
                    if (!sanPham.GiaBan.HasValue || sanPham.GiaBan.Value <= 0)
                    {
                        sanPham.GiaBan = 0;
                    }
                    if (!sanPham.SoLuongTon.HasValue || sanPham.SoLuongTon.Value < 0)
                    {
                        sanPham.SoLuongTon = 0;
                    }
                    if (string.IsNullOrWhiteSpace(sanPham.TinhTrang))
                    {
                        sanPham.TinhTrang = sanPham.SoLuongTon > 0 ? "Còn hàng" : "Hết hàng";
                    }

                    db.SANPHAMs.Add(sanPham);
                    db.SaveChanges();

                    // Chuẩn bị dữ liệu trả về để tránh circular references
                    db.Entry(sanPham).State = System.Data.Entity.EntityState.Detached;
                    sanPham.LOAIHANG = null;
                    sanPham.CTHDs = null;

                    return Ok(sanPham);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Lỗi khi tạo sản phẩm: {ex.Message}"));
            }
        }

        // PUT: api/SanPham/SP001
        [HttpPut]
        public IHttpActionResult UpdateSanPham(string id, [FromBody] SANPHAM sanPham)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest("Mã sản phẩm không được để trống");
                }

                if (sanPham == null)
                {
                    return BadRequest("Dữ liệu cập nhật không được để trống");
                }

                using (var db = new QL_SHOPFLOWEREntities())
                {
                    var existingProduct = db.SANPHAMs.FirstOrDefault(sp => sp.MaSP == id);
                    if (existingProduct == null)
                    {
                        return NotFound();
                    }

                    // Cập nhật các trường
                    if (!string.IsNullOrWhiteSpace(sanPham.TenSP))
                    {
                        existingProduct.TenSP = sanPham.TenSP;
                    }
                    if (sanPham.GiaBan.HasValue)
                    {
                        existingProduct.GiaBan = sanPham.GiaBan;
                    }
                    if (sanPham.AnhSP != null)
                    {
                        existingProduct.AnhSP = sanPham.AnhSP;
                    }
                    if (sanPham.MoTaSP != null)
                    {
                        existingProduct.MoTaSP = sanPham.MoTaSP;
                    }
                    if (sanPham.TinhTrang != null)
                    {
                        existingProduct.TinhTrang = sanPham.TinhTrang;
                    }
                    if (sanPham.ThuongHieu != null)
                    {
                        existingProduct.ThuongHieu = sanPham.ThuongHieu;
                    }
                    if (sanPham.SoLuongTon.HasValue)
                    {
                        existingProduct.SoLuongTon = sanPham.SoLuongTon;
                    }
                    if (!string.IsNullOrEmpty(sanPham.MaLoai))
                    {
                        var loaiExists = db.LOAIHANGs.Any(lh => lh.MaLoai == sanPham.MaLoai);
                        if (!loaiExists)
                        {
                            return BadRequest("Mã loại sản phẩm không tồn tại");
                        }
                        existingProduct.MaLoai = sanPham.MaLoai;
                    }

                    db.SaveChanges();

                    // Chuẩn bị dữ liệu trả về để tránh circular references
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    
                    var updatedProduct = db.SANPHAMs.AsNoTracking().FirstOrDefault(sp => sp.MaSP == id);
                    if (updatedProduct != null)
                    {
                        updatedProduct.LOAIHANG = null;
                        updatedProduct.CTHDs = null;
                    }

                    return Ok(updatedProduct);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Lỗi khi cập nhật sản phẩm: {ex.Message}"));
            }
        }

        private string GenerateMaSanPham(QL_SHOPFLOWEREntities db)
        {
            var allSanPham = db.SANPHAMs.ToList();

            if (allSanPham == null || !allSanPham.Any())
            {
                return "SP001     ";
            }

            int maxNumber = 0;
            foreach (var sanPham in allSanPham)
            {
                string maSP = sanPham.MaSP?.Trim() ?? "";
                if (maSP.Length >= 2 && maSP.StartsWith("SP"))
                {
                    string numberPart = maSP.Substring(2);
                    if (int.TryParse(numberPart, out int number))
                    {
                        if (number > maxNumber)
                        {
                            maxNumber = number;
                        }
                    }
                }
            }

            int newNumber = maxNumber + 1;
            string newMaSP = $"SP{newNumber:D3}";
            return newMaSP.PadRight(10);
        }
    }
}