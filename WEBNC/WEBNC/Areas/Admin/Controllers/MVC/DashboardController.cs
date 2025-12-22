using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEBNC.DataAccess.Data;

namespace WEBNC.Areas.Admin.Controllers.MVC
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DashboardController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalProduct = await _db.SanPham.CountAsync();
            ViewBag.LowStock = await _db.SanPham.CountAsync(x => x.soLuongHienCon > 0 && x.soLuongHienCon <= x.soLuongCanDuoi);
            ViewBag.OutStock = await _db.SanPham.CountAsync(x => x.soLuongHienCon <= 0);

            return View();
        }
    }
}
