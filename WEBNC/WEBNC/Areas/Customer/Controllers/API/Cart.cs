using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WEBNC.DataAccess.Repository.IRepository;
using WEBNC.Models;

namespace WEBNC.Areas.Customer.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // bắt buộc đăng nhập mới xem giỏ hàng
    public class Cart : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public Cart(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpPost("add")]
        public IActionResult AddToCart([FromBody] ChiTietGioHang cart)
        {
            if (cart == null || string.IsNullOrEmpty(cart.idSanPham))
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ" });
            }
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                return Unauthorized(new { message = "Bạn chưa đăng nhập" });

            string userId = claim.Value;
            cart.idNguoiDung = userId;
            cart.idChiTietGioHang = "GH" + Guid.NewGuid().ToString("N").Substring(0, 10);

            // Kiểm tra sản phẩm đã có trong giỏ chưa
            var cartFromDb = _unitOfWork.chiTietGioHang
                .GetFirstOrDefault(u => u.idNguoiDung == userId && u.idSanPham == cart.idSanPham);

            if (cartFromDb != null)
            {
                cartFromDb.soLuongTrongGio += cart.soLuongTrongGio;

                _unitOfWork.chiTietGioHang.Update(cartFromDb);
            }
            else
            {
                _unitOfWork.chiTietGioHang.Add(cart);
            }

            _unitOfWork.save();

            return Ok(new { message = "Đã thêm vào giỏ hàng" });
        }


        [HttpGet]
        public IActionResult GetUserCart()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return Unauthorized(new { message = "Bạn chưa đăng nhập" });
            }

            var chiTietGioHang = _unitOfWork.chiTietGioHang
                .GetAll(u => u.idNguoiDung == claim.Value, includeProperties: "SanPham");

            return Ok(new { data = chiTietGioHang });
        }
    }
}
