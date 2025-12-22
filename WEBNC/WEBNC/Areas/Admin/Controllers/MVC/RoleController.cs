using Microsoft.AspNetCore.Mvc;

namespace WEBNC.Areas.Admin.Controllers.MVC
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
