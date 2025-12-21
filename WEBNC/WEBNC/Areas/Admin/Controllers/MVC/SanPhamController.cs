using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEBNC.DataAccess.Data;
using WEBNC.Models;

namespace WEBNC.Areas.Admin.Controllers.MVC
{
    [Area("Admin")]
    public class SanPhamController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SanPhamController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(string? q)
        {
            q = q?.Trim();
            ViewBag.Q = q;

            var query = _db.SanPham
                .Include(x => x.congTy)
                .Include(x => x.LoaiSanPham)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(x =>
                    x.idSanPham.Contains(q) ||
                    x.tenSanPham.Contains(q) ||
                    (x.congTy != null && x.congTy.tenCongTy.Contains(q)) ||
                    (x.LoaiSanPham != null && x.LoaiSanPham.tenLoaiSanPham.Contains(q))
                );
            }

            var data = await query.OrderBy(x => x.idSanPham).ToListAsync();
            return View(data);
        }

        // ====== CREATE ======
        public async Task<IActionResult> Create()
        {
            await LoadDropDowns();
            return View(new SanPham());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SanPham model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropDowns();
                return View(model);
            }

            // tránh trùng id
            bool exists = await _db.SanPham.AnyAsync(x => x.idSanPham == model.idSanPham);
            if (exists)
            {
                ModelState.AddModelError("idSanPham", "Mã sản phẩm đã tồn tại!");
                await LoadDropDowns();
                return View(model);
            }

            _db.SanPham.Add(model);
            await _db.SaveChangesAsync();
            TempData["ok"] = "Thêm sản phẩm thành công!";
            return RedirectToAction(nameof(Index));
        }

        // ====== EDIT ======
        public async Task<IActionResult> Edit(string id)
        {
            var sp = await _db.SanPham.FindAsync(id);
            if (sp == null) return NotFound();

            await LoadDropDowns();
            return View(sp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, SanPham model)
        {
            if (id != model.idSanPham) return BadRequest();

            if (!ModelState.IsValid)
            {
                await LoadDropDowns();
                return View(model);
            }

            _db.SanPham.Update(model);
            await _db.SaveChangesAsync();

            TempData["ok"] = "Cập nhật sản phẩm thành công!";
            return RedirectToAction(nameof(Index));
        }

        // ====== DELETE ======
        public async Task<IActionResult> Delete(string id)
        {
            var sp = await _db.SanPham
                .Include(x => x.congTy)
                .Include(x => x.LoaiSanPham)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.idSanPham == id);

            if (sp == null) return NotFound();
            return View(sp);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var sp = await _db.SanPham.FindAsync(id);
            if (sp == null) return NotFound();

            _db.SanPham.Remove(sp);
            await _db.SaveChangesAsync();

            TempData["ok"] = "Xóa sản phẩm thành công!";
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropDowns()
        {
            ViewBag.CongTy = await _db.CongTy.AsNoTracking()
                .Select(x => new SelectListItem { Value = x.idCongTy, Text = x.tenCongTy })
                .ToListAsync();

            ViewBag.Loai = await _db.LoaiSanPham.AsNoTracking()
                .Select(x => new SelectListItem { Value = x.idLoaiSanPham, Text = x.tenLoaiSanPham })
                .ToListAsync();
        }
    }
}
