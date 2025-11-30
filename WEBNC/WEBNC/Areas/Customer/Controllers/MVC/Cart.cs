using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WEBNC.Models;

namespace WEBNC.Areas.Customer.Controllers.MVC
{
    [Area("Customer")]
    [Authorize]
    public class Cart : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
