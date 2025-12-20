using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEBNC.Models;

namespace WEBNC.Areas.Admin.Controllers.API
{
    [Area("Admin")]
    [Route("admin/api/[controller]")]
    [ApiController]
    public class SanPhamApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SanPhamApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /admin/api/SanPhamApi?q=
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? q)
        {
            var query = _context.SanPham
                .Include(x => x.congTy)
                .Include(x => x.LoaiSanPham)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(x => x.idSanPham.Contains(q) || x.tenSanPham.Contains(q));
            }

            var data = await query
                .OrderBy(x => x.idSanPham)
                .Select(x => new
                {
                    x.idSanPham,
                    x.tenSanPham,
                    x.gia,
                    x.soLuongHienCon,
                    x.imageURL,
                    CongTy = x.congTy != null ? x.congTy.tenCongTy : x.idCongTy,
                    Loai = x.LoaiSanPham != null ? x.LoaiSanPham.tenLoaiSanPham : x.idLoaiSanPham
                })
                .ToListAsync();

            return Ok(data);
        }

        // GET: /admin/api/SanPhamApi/SP01
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var sp = await _context.SanPham.FindAsync(id);
            if (sp == null) return NotFound();
            return Ok(sp);
        }

        // POST: /admin/api/SanPhamApi
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SanPham model)
        {
            if (model == null) return BadRequest();

            bool exists = await _context.SanPham.AnyAsync(x => x.idSanPham == model.idSanPham);
            if (exists) return Conflict(new { message = "Mã sản phẩm đã tồn tại." });

            _context.SanPham.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = model.idSanPham }, model);
        }

        // PUT: /admin/api/SanPhamApi/SP01
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] SanPham model)
        {
            if (model == null) return BadRequest();
            if (id != model.idSanPham) return BadRequest(new { message = "Id không khớp." });

            var sp = await _context.SanPham.FindAsync(id);
            if (sp == null) return NotFound();

            sp.idCongTy = model.idCongTy;
            sp.idLoaiSanPham = model.idLoaiSanPham;
            sp.tenSanPham = model.tenSanPham;
            sp.imageURL = model.imageURL;

            sp.gia = model.gia;
            sp.moTa = model.moTa;
            sp.imageLienQuan = model.imageLienQuan;
            sp.thongSoSanPham = model.thongSoSanPham;
            sp.soLuongHienCon = model.soLuongHienCon;
            sp.soLuongCanDuoi = model.soLuongCanDuoi;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Cập nhật thành công." });
        }

        // DELETE: /admin/api/SanPhamApi/SP01
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var sp = await _context.SanPham.FindAsync(id);
            if (sp == null) return NotFound();

            _context.SanPham.Remove(sp);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Xóa thành công." });
        }
    }
}
