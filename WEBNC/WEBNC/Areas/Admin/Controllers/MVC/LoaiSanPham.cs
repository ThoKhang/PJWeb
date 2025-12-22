using Microsoft.AspNetCore.Mvc;

namespace WEBNC.Areas.Admin.Controllers.MVC
{
    [Area("Admin")]
    public class LoaiSanPham : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert()
        {
            return View();
        }
        public IActionResult Delete()
        {
            return View();
        }
    }
}
