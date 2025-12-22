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

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return Ok(new { data = Enumerable.Empty<object>() });
            q = q.Trim();
            var products = _unitOfWork.SanPham.GetAll(
                u => u.tenSanPham.ToLower().Contains(q.ToLower())
                     || (u.moTa != null && u.moTa.ToLower().Contains(q.ToLower())),
                includeProperties: "LoaiSanPham"
            );
            var result = products.Select(p => new
            {
                id = p.idSanPham,
                tenSanPham = p.tenSanPham,
                gia = p.gia,
                imageURL = p.imageURL,
                loai = p.LoaiSanPham != null ? p.LoaiSanPham.tenLoaiSanPham : ""
            });
            return Ok(new { data = result });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var obj = _unitOfWork.LoaiSanPham.GetFirstOrDefault(x => x.idLoaiSanPham == id);
            if (obj == null)
                return NotFound();
            return Ok(obj);
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

                _unitOfWork.Save();
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
                _unitOfWork.Save();
                return Ok(new
                {
                    message = "Đã thêm sản phẩm vào giỏ hàng",
                    data = gioHang
                });
            }
        }
    }
}
