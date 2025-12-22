using Microsoft.AspNetCore.Mvc;

namespace WEBNC.Areas.Admin.Controllers.MVC
{
    [Area("Admin")]
    public class CongTyController : Controller
    {
        // /Admin/CongTy
        public IActionResult Index()
        {
            return View();
        }

        // /Admin/CongTy/Upsert?id=CT01
        public IActionResult Upsert(string? id)
        {
            ViewBag.Id = id;
            return View();
        }
    }
}
