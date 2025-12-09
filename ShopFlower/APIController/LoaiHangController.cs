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
    }
}