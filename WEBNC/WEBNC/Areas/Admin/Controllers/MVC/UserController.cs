using Microsoft.AspNetCore.Mvc;

namespace WEBNC.Areas.Admin.Controllers.MVC
{
    [Area("Admin")]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
