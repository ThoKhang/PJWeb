using Microsoft.AspNetCore.Mvc;

namespace WEBNC.Areas.Customer.Controllers.MVC
{
    public class DanhGiaController : Controller
    {
        [Area("Customer")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
