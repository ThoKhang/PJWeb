using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WEBNC.DataAccess.Repository.IRepository;
using WEBNC.Models;

namespace WEBNC.Areas.Customer.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DonDatHangController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DonDatHangController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Cập nhật DTO: Thêm DiaChi
        public class MuaHangRequest
        {
            public int SoLuong { get; set; }
            public string? Sdt { get; set; }
            public string? DiaChi { get; set; } // Mới thêm
        }

        [HttpPost("~/api/customer/products/{id}")]
        public IActionResult BuyNow([FromRoute] string id, [FromBody] MuaHangRequest request)
        {
            try
            {
                // 1. Kiểm tra cơ bản
                if (string.IsNullOrEmpty(id)) return BadRequest(new { success = false, message = "ID sản phẩm lỗi." });
                if (request.SoLuong <= 0) request.SoLuong = 1;

                var claim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (claim == null) return Unauthorized(new { success = false, message = "Hết phiên." });
                string userId = claim.Value;

                var sanPham = _unitOfWork.SanPham.GetFirstOrDefault(u => u.idSanPham == id);
                if (sanPham == null) return BadRequest(new { success = false, message = "Sản phẩm không tồn tại." });

                // 2. TÌM THÔNG TIN LIÊN LẠC (SĐT & ĐỊA CHỈ)
                // -----------------------------------------------------------
                string finalSdt = "";
                string finalDiaChi = "";

                // Ưu tiên lấy từ Client gửi lên
                if (!string.IsNullOrEmpty(request.Sdt)) finalSdt = request.Sdt;
                if (!string.IsNullOrEmpty(request.DiaChi)) finalDiaChi = request.DiaChi;


                // 3. KIỂM TRA LẦN CUỐI: Nếu thiếu 1 trong 2 -> Yêu cầu nhập
                if (string.IsNullOrEmpty(finalSdt) || string.IsNullOrEmpty(finalDiaChi))
                {
                    return Ok(new
                    {
                        success = false,
                        requiresInfo = true, // Cờ báo hiệu thiếu thông tin
                        missingSdt = string.IsNullOrEmpty(finalSdt), // Báo cụ thể thiếu cái nào
                        missingAddress = string.IsNullOrEmpty(finalDiaChi),
                        message = "Vui lòng cung cấp thông tin giao hàng."
                    });
                }
                // -----------------------------------------------------------

                // 4. TẠO ĐƠN HÀNG
                string suffix = new Random().Next(100, 999).ToString();
                string maDonHang = "DH" + suffix;

                DonDatHang donHang = new DonDatHang
                {
                    idDonDat = maDonHang,
                    idNguoiDung = userId,
                    ngayDat = DateTime.Now,
                    trangThai = "Chờ xác nhận",
                    thanhToan = "Chưa thanh toán",
                    sdtGiaoHang = finalSdt,
                    soNha = finalDiaChi, // Lưu địa chỉ chuẩn
                    ngayGiaoDuKien = DateTime.Now.AddDays(3)
                };

                _unitOfWork.DonDatHang.Add(donHang);

                ChiTietDonHang chiTiet = new ChiTietDonHang
                {
                    idDonDat = donHang.idDonDat,
                    idSanPham = id,
                    soluong = request.SoLuong,
                    donGia = sanPham.gia
                };

                _unitOfWork.ChiTietDonHang.Add(chiTiet);
                _unitOfWork.save();

                return Ok(new
                {
                    success = true,
                    message = "Đặt hàng thành công!",
                    redirectUrl = "/Customer/DonDatHang/Index"
                });
            }
            catch (Exception ex)
            {
                var errorMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return BadRequest(new { success = false, message = "Lỗi Server: " + errorMsg });
            }
        }

        // Các hàm lấy thông tin đơn hàng
        [HttpGet("get-my-orders")]
        public IActionResult GetMyOrders()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null) return Unauthorized(new { message = "Bạn chưa đăng nhập" });
            string userId = claim.Value;
            var donHangs = _unitOfWork.DonDatHang.GetAll(d => d.idNguoiDung == userId, includeProperties: "ChiTietDonHang");
            var result = donHangs.Select(d => new {
                idDonDat = d.idDonDat,
                ngayDat = d.ngayDat,
                trangThai = d.trangThai,
                tongTien = d.ChiTietDonHang != null ? d.ChiTietDonHang.Sum(c => c.soluong * c.donGia) : 0
            }).OrderByDescending(x => x.ngayDat);
            return Ok(new { data = result });
        }

        [HttpGet("details/{id}")]
        public IActionResult GetOrderDetails(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null) return Unauthorized();
            string userId = claim.Value;
            var donDat = _unitOfWork.DonDatHang.GetFirstOrDefault(d => d.idDonDat == id && d.idNguoiDung == userId);
            if (donDat == null) return NotFound(new { success = false, message = "Không tìm thấy đơn hàng" });
            var chiTiet = _unitOfWork.ChiTietDonHang.GetAll(c => c.idDonDat == id, includeProperties: "SanPham");
            var chiTietResult = chiTiet.Select(c => new {
                tenSanPham = c.SanPham != null ? c.SanPham.tenSanPham : "Sản phẩm lỗi",
                hinhAnh = c.SanPham != null ? c.SanPham.imageURL : "",
                soLuong = c.soluong,
                donGia = c.donGia,
                thanhTien = c.soluong * c.donGia
            });
            return Ok(new { success = true, orderInfo = new { idDonDat = donDat.idDonDat, ngayDat = donDat.ngayDat, trangThai = donDat.trangThai, diaChi = donDat.soNha, sdt = donDat.sdtGiaoHang, tongTien = chiTietResult.Sum(x => x.thanhTien) }, orderDetails = chiTietResult });
        }

        // =========================================================================
        // 4. API HỦY ĐƠN HÀNG (Chỉ cho phép hủy nếu trạng thái là 'Chờ xác nhận')
        // URL: POST /api/DonDatHang/cancel/{id}
        // =========================================================================
        [HttpPost("cancel/{id}")]
        public IActionResult CancelOrder(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id)) return BadRequest(new { success = false, message = "ID không hợp lệ" });

                var claim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (claim == null) return Unauthorized(new { success = false, message = "Bạn chưa đăng nhập" });
                string userId = claim.Value;

                // Tìm đơn hàng (Phải đúng ID đơn và đúng User đó mới được hủy)
                var donHang = _unitOfWork.DonDatHang.GetFirstOrDefault(u => u.idDonDat == id && u.idNguoiDung == userId);

                if (donHang == null)
                {
                    return NotFound(new { success = false, message = "Không tìm thấy đơn hàng." });
                }

                // Kiểm tra trạng thái: Chỉ "Chờ xác nhận" mới được hủy
                // Nếu "Đang giao" hoặc "Đã giao" thì không được hủy
                if (donHang.trangThai != "Chờ xác nhận")
                {
                    return BadRequest(new { success = false, message = "Đơn hàng đã được xử lý hoặc đang giao, không thể hủy." });
                }

                // Cập nhật trạng thái
                donHang.trangThai = "Đã hủy"; // Cần đảm bảo giá trị này khớp với Constraint trong SQL của bạn
                _unitOfWork.DonDatHang.Update(donHang);
                _unitOfWork.save();

                return Ok(new { success = true, message = "Đã hủy đơn hàng thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Lỗi hệ thống: " + ex.Message });
            }
        }
    }
}