using Microsoft.AspNetCore.Mvc;
using WEBNC.Models;
using WEBNC.Services;
using WEBNC.Services.Momo;
using WEBNC.DataAccess.Repository.IRepository;
using System.Security.Claims;

namespace WEBNC.Areas.Customer.Controllers.API
{
    [Area("Customer")]
    public class PaymentController : Controller
    {
        private readonly IMomoService _momoService;
        private readonly IUnitOfWork _unitOfWork;
        public PaymentController(IMomoService momoPaymentService, IUnitOfWork unitOfWork)
        {
            _momoService = momoPaymentService;
            _unitOfWork = unitOfWork;
        }
        [HttpPost]
        public async Task<IActionResult> CreatePaymentMomo(OrderInfoModel model)
        {
            if (model.Amount <= 0)
            {
                var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { success = false, message = "Bạn chưa đăng nhập" });
                }

                var items = _unitOfWork.chiTietGioHang.GetAll(u => u.idNguoiDung == userId, includeProperties: "SanPham");
                decimal subtotal = items.Sum(i => i.SanPham.gia * i.soLuongTrongGio);
                decimal shipping = subtotal > 0 ? 30000m : 0m;
                model.Amount = (double)(subtotal + shipping);
                if (model.Amount > 0 && model.Amount < 1000)
                {
                    model.Amount = 1000;
                }

                if (model.Amount <= 0)
                {
                    return BadRequest(new { success = false, message = "Giỏ hàng trống hoặc số tiền không hợp lệ" });
                }
            }
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var returnUrl = baseUrl + "/Customer/Payment/MomoPaymentExecute";
            var notifyUrl = returnUrl;
            var response = await _momoService.CreatePaymentMomo(model, returnUrl, notifyUrl);
            if (response == null || string.IsNullOrEmpty(response.PayUrl))
            {
                var message = response?.LocalMessage ?? response?.Message ?? "Không nhận được PayUrl từ Momo";
                return BadRequest(new { success = false, message, errorCode = response?.ErrorCode, data = response });
            }
            return Redirect(response.PayUrl);
        }
        [HttpGet]
        public IActionResult MomoPaymentExecute()
        {
            var momo = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
            bool isSuccess = momo.ErrorCode == "0" || momo.ResultCode == "0";
            if (!isSuccess)
            {
                var msg = momo.LocalMessage ?? momo.Message ?? "Giao dịch đã hủy";
                var code = momo.ErrorCode ?? momo.ResultCode ?? "";
                return Redirect($"/Customer/Cart/Index?cancel=1&code={Uri.EscapeDataString(code)}&msg={Uri.EscapeDataString(msg)}");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { success = false, message = "Hết phiên. Vui lòng đăng nhập lại." });
            }

            var cartItems = _unitOfWork.chiTietGioHang.GetAll(u => u.idNguoiDung == userId, includeProperties: "SanPham").ToList();
            if (!cartItems.Any())
            {
                return BadRequest(new { success = false, message = "Giỏ hàng trống, không thể tạo đơn." });
            }

            string suffix = new Random().Next(100, 999).ToString();
            string maDonHang = "DH" + suffix;

            var userPhone = User.Identity?.Name ?? ""; // placeholder, nên lấy từ ApplicationUser nếu có
            var sdt = "0000000000";
            var diaChi = "Chưa cập nhật";

            var donHang = new DonDatHang
            {
                idDonDat = maDonHang,
                idNguoiDung = userId,
                ngayDat = DateTime.Now,
                trangThai = "Chờ xác nhận",
                thanhToan = "Đã thanh toán",
                ngayThanhToan = DateTime.Now,
                sdtGiaoHang = sdt,
                soNha = diaChi,
                ngayGiaoDuKien = DateTime.Now.AddDays(3)
            };
            _unitOfWork.DonDatHang.Add(donHang);

            foreach (var item in cartItems)
            {
                var ct = new ChiTietDonHang
                {
                    idDonDat = maDonHang,
                    idSanPham = item.idSanPham,
                    soluong = item.soLuongTrongGio,
                    donGia = item.SanPham.gia
                };
                _unitOfWork.ChiTietDonHang.Add(ct);
            }

            foreach (var item in cartItems)
            {
                _unitOfWork.chiTietGioHang.Remove(item);
            }

            _unitOfWork.save();

            return Redirect("/Customer/DonDatHang/Index");
        }
    }
}
