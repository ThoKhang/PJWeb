using Microsoft.AspNetCore.Mvc;

namespace WEBNC.Areas.Admin.Controllers.MVC
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        // GET: /Admin/Dashboard
        public IActionResult Index()
        {
            return View();
        }
    }
}
