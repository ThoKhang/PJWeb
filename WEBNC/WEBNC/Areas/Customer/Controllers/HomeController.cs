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
        [HttpGet]
        public IActionResult TopSanPhamBanChay()
        {
            var topSanPham = _unitOfWork.SanPham.LayTop10SanPhamBanChay();
            return Json(new { data = topSanPham });
        }
        [HttpPost("giohang/tang")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult AddToCart([FromBody] ChiTietGioHang gioHang)
        {
            if (gioHang == null || string.IsNullOrEmpty(gioHang.idSanPham))
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });

            // Lấy id người dùng đang đăng nhập
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            gioHang.idNguoiDung = claim.Value;
            gioHang.idChiTietGioHang = null;


            // Kiểm tra sản phẩm có trong giỏ chưa
            var gioHangDb = _unitOfWork.chiTietGioHang.GetFirstOrDefault(
                u => u.idNguoiDung == gioHang.idNguoiDung &&
                     u.idSanPham == gioHang.idSanPham
            );

            if (gioHangDb != null)
            {
                // Tăng số lượng
                _unitOfWork.chiTietGioHang.IncrementCount(gioHangDb, gioHang.soLuongTrongGio);
                _unitOfWork.chiTietGioHang.Update(gioHangDb);

                _unitOfWork.save();
                return Ok(new
                {
                    message = "Cập nhật số lượng sản phẩm trong giỏ hàng",
                    data = gioHangDb
                });
            }
            else
            {
                // Thêm mới
                _unitOfWork.chiTietGioHang.Add(gioHang);
                _unitOfWork.save();

                return Ok(new
                {
                    message = "Đã thêm sản phẩm vào giỏ hàng",
                    data = gioHang
                });
            }
        }

        [HttpPut("giohang/giam")]
        [Authorize]
        public IActionResult GiamSoLuong([FromBody] ChiTietGioHang gioHang)
        {
            if (gioHang == null || string.IsNullOrEmpty(gioHang.idSanPham))
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var gioHangDb = _unitOfWork.chiTietGioHang.GetFirstOrDefault(
                u => u.idNguoiDung == claim.Value &&
                     u.idSanPham == gioHang.idSanPham
            );

            if (gioHangDb == null)
                return NotFound(new { message = "Không tìm thấy sản phẩm trong giỏ hàng" });

            // Nếu số lượng = 1 → xóa luôn
            if (gioHangDb.soLuongTrongGio <= 1)
            {
                _unitOfWork.chiTietGioHang.Remove(gioHangDb);
                _unitOfWork.save();
                return Ok(new { message = "Đã xóa sản phẩm khỏi giỏ hàng" });
            }

            // Nếu số lượng > 1 → giảm 1
            _unitOfWork.chiTietGioHang.DecrementCount(gioHangDb, 1);
            _unitOfWork.chiTietGioHang.Update(gioHangDb);
            _unitOfWork.save();

            return Ok(new
            {
                message = "Đã giảm số lượng",
                data = gioHangDb
            });
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
