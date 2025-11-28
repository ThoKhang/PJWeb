using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WEBNC.DataAccess.Repository.IRepository;

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
