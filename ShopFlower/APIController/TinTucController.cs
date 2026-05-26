using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ShopFlower.Models;

namespace ShopFlower.APIController
{
    public class TinTucController : ApiController
    {
        // GET: api/TinTuc
        [HttpGet]
        public IHttpActionResult GetAllTinTuc()
        {
            try
            {
                using (var db = new QL_SHOPFLOWEREntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;

                    var tinTucs = db.TINTUCs.ToList();

                    return Ok(tinTucs);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Lỗi: {ex.Message}"));
            }
        }

        // GET: api/TinTuc/TT001
        [HttpGet]
        public IHttpActionResult GetTinTucById(string id)
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

                    var tinTuc = db.TINTUCs.FirstOrDefault(s => s.MATT == id);

                    if (tinTuc == null)
                    {
                        return NotFound();
                    }

                    return Ok(tinTuc);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Lỗi: {ex.Message}"));
            }
        }

        // POST: api/TinTuc
        [HttpPost]
        public IHttpActionResult CreateTinTuc([FromBody] TINTUC tinTuc)
        {
            try
            {
                if (tinTuc == null)
                {
                    return BadRequest("Dữ liệu tin tức không được để trống");
                }

                if (string.IsNullOrWhiteSpace(tinTuc.TIEUDE))
                {
                    return BadRequest("Tiêu đề tin tức không được để trống");
                }

                using (var db = new QL_SHOPFLOWEREntities())
                {
                    // Tự động sinh mã tin tức mới
                    tinTuc.MATT = GenerateMaTinTuc(db);
                    tinTuc.NGAYTHEM = DateTime.Now;

                    db.TINTUCs.Add(tinTuc);
                    db.SaveChanges();

                    // Chuẩn bị dữ liệu trả về
                    db.Entry(tinTuc).State = System.Data.Entity.EntityState.Detached;

                    return Ok(tinTuc);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Lỗi khi đăng tin tức: {ex.Message}"));
            }
        }

        // PUT: api/TinTuc/TT001
        [HttpPut]
        public IHttpActionResult UpdateTinTuc(string id, [FromBody] TINTUC tinTuc)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest("Mã tin tức không được để trống");
                }

                if (tinTuc == null)
                {
                    return BadRequest("Dữ liệu cập nhật không được để trống");
                }

                using (var db = new QL_SHOPFLOWEREntities())
                {
                    // Trim khóa chính vì trong DB là CHAR(10) nên cần so sánh linh hoạt
                    var existingTin = db.TINTUCs.FirstOrDefault(t => t.MATT.Trim() == id.Trim());
                    if (existingTin == null)
                    {
                        return NotFound();
                    }

                    if (!string.IsNullOrWhiteSpace(tinTuc.TIEUDE))
                    {
                        existingTin.TIEUDE = tinTuc.TIEUDE;
                    }
                    if (tinTuc.ANHBIA != null)
                    {
                        existingTin.ANHBIA = tinTuc.ANHBIA;
                    }
                    if (tinTuc.MOTA != null)
                    {
                        existingTin.MOTA = tinTuc.MOTA;
                    }

                    db.SaveChanges();

                    // Chuẩn bị dữ liệu trả về
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;

                    var updatedTin = db.TINTUCs.AsNoTracking().FirstOrDefault(t => t.MATT == existingTin.MATT);

                    return Ok(updatedTin);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Lỗi khi cập nhật tin tức: {ex.Message}"));
            }
        }

        private string GenerateMaTinTuc(QL_SHOPFLOWEREntities db)
        {
            var allTinTuc = db.TINTUCs.ToList();

            if (allTinTuc == null || !allTinTuc.Any())
            {
                return "TT001     ";
            }

            int maxNumber = 0;
            foreach (var t in allTinTuc)
            {
                string maTT = t.MATT?.Trim() ?? "";
                if (maTT.Length >= 2 && maTT.StartsWith("TT"))
                {
                    string numberPart = maTT.Substring(2);
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
            string newMaTT = $"TT{newNumber:D3}";
            return newMaTT.PadRight(10);
        }
    }
}