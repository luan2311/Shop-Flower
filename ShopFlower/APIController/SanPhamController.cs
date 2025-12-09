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
    }
}