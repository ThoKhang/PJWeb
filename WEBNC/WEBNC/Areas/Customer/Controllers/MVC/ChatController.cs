using Microsoft.AspNetCore.Mvc;
namespace WEBNC.Areas.Customer.Controllers.MVC
{
    [Area("Customer")]
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
