using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using WEBNC.DataAccess.Repository.IRepository;
using WEBNC.Models;

namespace WEBNC.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            /*
            // Nếu đã đăng nhập và có role Admin thì tự chuyển vào khu vực Admin
            if (User.Identity != null
                && User.Identity.IsAuthenticated
                && User.IsInRole("Admin"))
            {
                // chuyển sang trang admin (đang dùng RoleController làm dashboard)
                return RedirectToAction("Index", "Role", new { area = "Admin" });
                // hoặc Home admin nếu bạn có:
                // return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

            // User thường hoặc chưa đăng nhập: vào trang mua bán bình thường
            */
            return View();

        }
        public IActionResult Details()
        {
            return View();
        }
    }
}