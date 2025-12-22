using Microsoft.AspNetCore.Mvc;
using WEBNC.DataAccess.Repository.IRepository;
using WEBNC.Models;

namespace WEBNC.Areas.Customer.Controllers.MVC
{
    [Area("Customer")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Search(string query, string category = "all")
        {
            var keyword = (query ?? "").Trim();

            var products = string.IsNullOrEmpty(keyword)
                ? _unitOfWork.SanPham.GetAll(includeProperties: "LoaiSanPham")
                : _unitOfWork.SanPham.GetAll(
                    u => u.tenSanPham.ToLower().Contains(keyword.ToLower())
                         || (u.moTa != null && u.moTa.ToLower().Contains(keyword.ToLower())),
                    includeProperties: "LoaiSanPham"
                  );

            if (!string.IsNullOrEmpty(category) && category != "all")
            {
                var cat = category.Trim().ToLower();
                products = products.Where(p =>
                    p.LoaiSanPham != null &&
                    p.LoaiSanPham.tenLoaiSanPham != null &&
                    p.LoaiSanPham.tenLoaiSanPham.Trim().ToLower() == cat
                );
            }

            ViewBag.SearchQuery = keyword;
            ViewBag.SelectedCategory = category;

            return View("SearchResults", products);
        }
    }
}
