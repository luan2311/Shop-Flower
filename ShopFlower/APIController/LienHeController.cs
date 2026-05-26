using ShopFlower.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web.Http;

namespace ShopFlower.APIController
{
    public class LienHeController : ApiController
    {
        // GET: api/LienHe
        [HttpGet]
        public IHttpActionResult GetAllLienHe()
        {
            try
            {
                using (var db = new QL_SHOPFLOWEREntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;

                    var lienHes = db.LIENHEs.ToList();
                    return Ok(lienHes);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Lỗi khi lấy danh sách liên hệ: {ex.Message}"));
            }
        }

        // POST: api/LienHe
        [HttpPost]
        public IHttpActionResult CreateLienHe([FromBody] LIENHE model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest("Dữ liệu liên hệ không được để trống");
                }

                if (string.IsNullOrWhiteSpace(model.HOTEN))
                {
                    return BadRequest("Họ tên không được để trống");
                }

                using (var db = new QL_SHOPFLOWEREntities())
                {
                    var maLHParam = new ObjectParameter("MaLH", typeof(string));

                    // Gọi Stored Procedure
                    int result = db.sp_ThemLienHe(
                        model.HOTEN,
                        model.EMAIL,
                        model.DIENTHOAI,
                        model.NOIDUNG,
                        maLHParam
                    );

                    string maLH = maLHParam.Value?.ToString();

                    if (result >= 0 || !string.IsNullOrEmpty(maLH))
                    {
                        var createdLienHe = db.LIENHEs.FirstOrDefault(lh => lh.HOTEN == model.HOTEN && lh.EMAIL == model.EMAIL);
                        return Ok(createdLienHe != null ? createdLienHe : model);
                    }
                    else
                    {
                        return BadRequest("Gửi liên hệ thất bại");
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Lỗi khi gửi liên hệ: {ex.Message}"));
            }
        }

        // DELETE: api/LienHe/5
        [HttpDelete]
        public IHttpActionResult DeleteLienHe(int id)
        {
            try
            {
                using (var db = new QL_SHOPFLOWEREntities())
                {
                    var lienHe = db.LIENHEs.Find(id);
                    if (lienHe == null)
                    {
                        return NotFound();
                    }

                    db.LIENHEs.Remove(lienHe);
                    db.SaveChanges();

                    return Ok(new { success = true, message = "Xóa liên hệ thành công" });
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Lỗi khi xóa liên hệ: {ex.Message}"));
            }
        }
    }
}
