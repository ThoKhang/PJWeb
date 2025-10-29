using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Web.DataAccess.Repository.IRepository;
using WEBNC.Models;

namespace WEBNC.Areas.Customer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<SanPham> sanPhamList = _unitOfWork.SanPham.GetAll(includeProperties:"LoaiSanPham");
            return Json(new {data=sanPhamList});
        }
        [HttpGet]
        public IActionResult SanPham(string? id)
        {
            if (id == null || id == "")
                return BadRequest();
            var sanPham = _unitOfWork.SanPham.GetFirstOrDefault(item => item.idSanPham == id);
            if (sanPham == null)
                return NotFound();
            return Json(new {data=sanPham});
        }
        public IActionResult Details()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
