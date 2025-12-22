using Microsoft.AspNetCore.Mvc;
using WEBNC.DataAccess.Repository.IRepository;
using WEBNC.Models;

namespace WEBNC.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Trang liệt kê tất cả sản phẩm (nếu cần)
        public IActionResult Index()
        {
            var products = _unitOfWork.SanPham.GetAll(includeProperties: "LoaiSanPham");
            return View(products);
        }

        // /Customer/Product/Search?searchQuery=abc&category=Điện thoại
        [HttpGet]
        public IActionResult Search(string? searchQuery, string? category)
        {
            var products = _unitOfWork.SanPham.GetAll(includeProperties: "LoaiSanPham");

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var q = searchQuery.Trim().ToLower();
                products = products.Where(p =>
                    (!string.IsNullOrEmpty(p.tenSanPham) && p.tenSanPham.ToLower().Contains(q)) ||
                    (!string.IsNullOrEmpty(p.moTa) && p.moTa.ToLower().Contains(q)));
            }

            if (!string.IsNullOrWhiteSpace(category) && category != "Tất cả")
            {
                var c = category.Trim().ToLower();
                products = products.Where(p =>
                    p.LoaiSanPham != null &&
                    !string.IsNullOrEmpty(p.LoaiSanPham.tenLoaiSanPham) &&
                    p.LoaiSanPham.tenLoaiSanPham.ToLower() == c);
            }

            ViewBag.SearchQuery = searchQuery;
            ViewBag.SelectedCategory = category;

            return View("Search", products);
        }
    }
}