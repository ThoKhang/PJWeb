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
    public class CartController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartController(IUnitOfWork unitOfWork)
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
        [HttpGet]
        [Route("/api/cart/count")]
        public IActionResult GetCartCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Ok(new { count = 0 });

            int count = _unitOfWork.chiTietGioHang.GetAll(u => u.idNguoiDung == userId).Count();

            return Ok(new { count });
        }
        [HttpDelete]
        [Route("/api/cart/{id}")]
        public IActionResult DeleteCart(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { success = false, message = "Bạn chưa đăng nhập" });
            var cartItem = _unitOfWork.chiTietGioHang.GetFirstOrDefault(x => x.idNguoiDung == userId && x.idSanPham == id);
            if (cartItem == null)
                return NotFound(new { success = false, message = "Không tìm thấy sản phẩm trong giỏ hàng" });
            _unitOfWork.chiTietGioHang.Remove(cartItem);
            _unitOfWork.save();
            return Ok(new { success = true, message = "Đã xóa sản phẩm khỏi giỏ hàng" });
        }

    }
}
