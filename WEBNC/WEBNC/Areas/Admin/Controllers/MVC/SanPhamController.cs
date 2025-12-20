using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEBNC.Models;

namespace WEBNC.Areas.Admin.Controllers.MVC
{
    [Area("Admin")]
    public class SanPhamController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SanPhamController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? q)
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

            ViewBag.Q = q;
            return View(await query.OrderBy(x => x.idSanPham).ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();
            return View(new SanPham());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SanPham model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View(model);
            }

            bool exists = await _context.SanPham.AnyAsync(x => x.idSanPham == model.idSanPham);
            if (exists)
            {
                ModelState.AddModelError("idSanPham", "Mã sản phẩm đã tồn tại.");
                await LoadDropdowns(model.idCongTy, model.idLoaiSanPham);
                return View(model);
            }

            _context.SanPham.Add(model);
            await _context.SaveChangesAsync();
            TempData["ok"] = "Thêm sản phẩm thành công!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var sp = await _context.SanPham.FindAsync(id);
            if (sp == null) return NotFound();

            await LoadDropdowns(sp.idCongTy, sp.idLoaiSanPham);
            return View(sp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, SanPham model)
        {
            if (id != model.idSanPham) return BadRequest();

            if (!ModelState.IsValid)
            {
                await LoadDropdowns(model.idCongTy, model.idLoaiSanPham);
                return View(model);
            }

            _context.Update(model);
            await _context.SaveChangesAsync();
            TempData["ok"] = "Cập nhật sản phẩm thành công!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var sp = await _context.SanPham
                .Include(x => x.congTy)
                .Include(x => x.LoaiSanPham)
                .FirstOrDefaultAsync(x => x.idSanPham == id);

            if (sp == null) return NotFound();
            return View(sp);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var sp = await _context.SanPham.FindAsync(id);
            if (sp == null) return NotFound();

            _context.SanPham.Remove(sp);
            await _context.SaveChangesAsync();
            TempData["ok"] = "Xóa sản phẩm thành công!";
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropdowns(string? selectedCongTy = null, string? selectedLoai = null)
        {
            ViewBag.CongTy = new SelectList(
                await _context.CongTy.OrderBy(x => x.tenCongTy).ToListAsync(),
                "idCongTy", "tenCongTy", selectedCongTy);

            ViewBag.LoaiSanPham = new SelectList(
                await _context.LoaiSanPham.OrderBy(x => x.tenLoaiSanPham).ToListAsync(),
                "idLoaiSanPham", "tenLoaiSanPham", selectedLoai);
        }
    }
}
