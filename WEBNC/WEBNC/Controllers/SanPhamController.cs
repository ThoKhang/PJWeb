using Microsoft.AspNetCore.Mvc;
using WEBNC.Data;

namespace WEBNC.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly ApplicationDbContext _db;
        public SanPhamController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Models.SanPham> objList = _db.SanPham.ToList();
            return View(objList);
        }
    }
}
