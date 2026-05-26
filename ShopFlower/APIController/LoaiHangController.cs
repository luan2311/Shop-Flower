using ShopFlower.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ShopFlower.APIController
{
    public class LoaiHangController : ApiController
    {
        // GET: api/LoaiHang
        [HttpGet]
        public IHttpActionResult GetAllLoaiHang()
        {
            try
            {
                using (var db = new QL_SHOPFLOWEREntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;

                    var loaiHangs = db.LOAIHANGs.ToList();

                    // Xóa navigation properties để tránh circular reference
                    foreach (var lh in loaiHangs)
                    {
                        lh.SANPHAMs = null;
                    }

                    return Ok(loaiHangs);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Lỗi: {ex.Message}"));
            }
        }

        // GET: api/LoaiHang/LH001
        [HttpGet]
        public IHttpActionResult GetLoaiHangById(string id)
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

                    var loaiHang = db.LOAIHANGs.FirstOrDefault(s => s.MaLoai == id);

                    if (loaiHang == null)
                    {
                        return NotFound();
                    }

                    // Xóa navigation properties
                    loaiHang.SANPHAMs = null;

                    return Ok(loaiHang);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Lỗi: {ex.Message}"));
            }
        }

        // POST: api/LoaiHang
        [HttpPost]
        public IHttpActionResult CreateLoaiHang([FromBody] LOAIHANG loaiHang)
        {
            try
            {
                if (loaiHang == null)
                {
                    return BadRequest("Dữ liệu danh mục không được để trống");
                }

                if (string.IsNullOrWhiteSpace(loaiHang.TenLoai))
                {
                    return BadRequest("Tên danh mục không được để trống");
                }

                using (var db = new QL_SHOPFLOWEREntities())
                {
                    // Tự động sinh mã danh mục mới
                    loaiHang.MaLoai = GenerateMaLoaiHang(db);

                    db.LOAIHANGs.Add(loaiHang);
                    db.SaveChanges();

                    // Chuẩn bị dữ liệu trả về để tránh circular references
                    db.Entry(loaiHang).State = System.Data.Entity.EntityState.Detached;
                    loaiHang.SANPHAMs = null;

                    return Ok(loaiHang);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Lỗi khi tạo danh mục: {ex.Message}"));
            }
        }

        // PUT: api/LoaiHang/LH001
        [HttpPut]
        public IHttpActionResult UpdateLoaiHang(string id, [FromBody] LOAIHANG loaiHang)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest("Mã danh mục không được để trống");
                }

                if (loaiHang == null)
                {
                    return BadRequest("Dữ liệu cập nhật không được để trống");
                }

                using (var db = new QL_SHOPFLOWEREntities())
                {
                    var existingLoai = db.LOAIHANGs.FirstOrDefault(lh => lh.MaLoai == id);
                    if (existingLoai == null)
                    {
                        return NotFound();
                    }

                    if (!string.IsNullOrWhiteSpace(loaiHang.TenLoai))
                    {
                        existingLoai.TenLoai = loaiHang.TenLoai;
                    }

                    db.SaveChanges();

                    // Chuẩn bị dữ liệu trả về
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;

                    var updatedLoai = db.LOAIHANGs.AsNoTracking().FirstOrDefault(lh => lh.MaLoai == id);
                    if (updatedLoai != null)
                    {
                        updatedLoai.SANPHAMs = null;
                    }

                    return Ok(updatedLoai);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Lỗi khi cập nhật danh mục: {ex.Message}"));
            }
        }

        private string GenerateMaLoaiHang(QL_SHOPFLOWEREntities db)
        {
            var allLoaiHang = db.LOAIHANGs.ToList();

            if (allLoaiHang == null || !allLoaiHang.Any())
            {
                return "LH001     ";
            }

            int maxNumber = 0;
            foreach (var lh in allLoaiHang)
            {
                string maLH = lh.MaLoai?.Trim() ?? "";
                if (maLH.Length >= 2 && maLH.StartsWith("LH"))
                {
                    string numberPart = maLH.Substring(2);
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
            string newMaLH = $"LH{newNumber:D3}";
            return newMaLH.PadRight(10);
        }
    }
}