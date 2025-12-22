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
        public IActionResult Create()
        {
            return View("Upsert");
        }
        public IActionResult Edit(string id)
        {
            return View("Upsert");
        }
    }
}
