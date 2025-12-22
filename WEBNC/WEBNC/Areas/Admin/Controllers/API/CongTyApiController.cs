using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WEBNC.Areas.Admin.Controllers.API
{
    [Area("Admin")]
    [Route("api/admin/congty")]
    [ApiController]
    public class CongTyApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public CongTyApiController(ApplicationDbContext db) => _db = db;

        // GET: /api/admin/congty
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _db.CongTy
                .AsNoTracking()
                .OrderBy(x => x.tenCongTy)
                .Select(x => new
                {
                    x.idCongTy,
                    x.tenCongTy
                })
                .ToListAsync();

            return Ok(data);
        }

        // GET: /api/admin/congty/CT01
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var item = await _db.CongTy
                .AsNoTracking()
                .Where(x => x.idCongTy == id)
                .Select(x => new { x.idCongTy, x.tenCongTy })
                .FirstOrDefaultAsync();

            if (item == null) return NotFound();
            return Ok(item);
        }
    }
}
