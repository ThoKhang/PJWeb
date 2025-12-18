using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WEBNC.DataAccess.Repository.IRepository;
using WEBNC.Models;

namespace WEBNC.Areas.Customer.Controllers.API
{
    [ApiController]
    [Route("api/customer/products")]
    public class SanPhamController : ControllerBase
    {
        private readonly ILogger<SanPhamController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public SanPhamController(ILogger<SanPhamController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAll(string? loai = "")
        {
            if (string.IsNullOrEmpty(loai))
            {
                var all = _unitOfWork.SanPham.GetAll(includeProperties: "LoaiSanPham");
                return Ok(new { data = all });
            }

            var filtered = _unitOfWork.SanPham.GetAll(
                u => u.LoaiSanPham.tenLoaiSanPham == loai,
                includeProperties: "LoaiSanPham"
            );

            return Ok(new { data = filtered });
        }

        [HttpGet("{id}")]
        public IActionResult SanPhamById(string? id)
        {
            if (id == null || id == "")
                return BadRequest();
            var sanPham = _unitOfWork.SanPham.GetFirstOrDefault(item => item.idSanPham == id);
            if (sanPham == null)
                return NotFound();
            return Ok(new { data = sanPham });
        }
        [HttpGet("top")]
        public IActionResult TopSanPhamBanChay()
        {
            var topSanPham = _unitOfWork.SanPham.LayTop10SanPhamBanChay();
            return Ok(new { data = topSanPham });
        }
        [HttpPost("giohang")]
        //[ValidateAntiForgeryToken]
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
    }
}