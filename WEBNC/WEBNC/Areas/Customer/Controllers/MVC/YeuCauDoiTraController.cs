using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WEBNC.Areas.Customer.Controllers.MVC
{
    [Area("Customer")]
    [Authorize]
    public class YeuCauDoiTraController : Controller
    {
        // GET: /Customer/YeuCauDoiTra
        public IActionResult Index()
        {
            return View();
        }

        // Hiện lịch sử yêu cầu đổi trả
        [HttpGet]
        public IActionResult History()
        {
            return View();
        }
    }

}