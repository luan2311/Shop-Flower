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
    }
}