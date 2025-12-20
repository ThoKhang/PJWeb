using Microsoft.AspNetCore.Mvc;
using WEBNC.Models;
using WEBNC.Services.Momo;
using WEBNC.DataAccess.Repository.IRepository;
using System.Security.Claims;
using Newtonsoft.Json;

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

        // =============================
        // 1) TẠO THANH TOÁN MOMO
        // =============================
        [HttpPost]
        public async Task<IActionResult> CreatePaymentMomo(OrderInfoModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { success = false, message = "Bạn chưa đăng nhập" });

            // Tính tiền giỏ hàng
            var items = _unitOfWork.chiTietGioHang.GetAll(
                u => u.idNguoiDung == userId,
                includeProperties: "SanPham"
            );

            decimal subtotal = items.Sum(i => i.SanPham.gia * i.soLuongTrongGio);
            decimal shipping = subtotal > 0 ? 30000m : 0m;

            model.Amount = (double)(subtotal + shipping);
            if (model.Amount <= 0)
                return BadRequest(new { success = false, message = "Giỏ hàng trống hoặc số tiền không hợp lệ" });

            // Tạo URL callback
            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            var returnUrl = baseUrl + "/Customer/Payment/MomoPaymentExecute"; // chỉ để hiển thị
            var notifyUrl = baseUrl + "/Customer/Payment/MomoNotify";        // nơi LƯU ĐƠN thật sự

            var response = await _momoService.CreatePaymentMomo(model, returnUrl, notifyUrl);

            if (response == null || string.IsNullOrEmpty(response.PayUrl))
                return BadRequest(new { success = false, message = "Không nhận được PayUrl từ Momo" });

            return Redirect(response.PayUrl);
        }


        // =============================
        // 2) RETURN URL (KHÔNG LƯU ĐƠN)
        // =============================
        [HttpGet]
        public IActionResult MomoPaymentExecute()
        {
            var momo = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
            bool isSuccess = momo.ErrorCode == "0" || momo.ResultCode == "0";

            if (!isSuccess)
            {
                var msg = momo.LocalMessage ?? momo.Message ?? "Giao dịch đã hủy";
                var code = momo.ErrorCode ?? momo.ResultCode ?? "";
                return Redirect($"/Customer/Cart/Index?cancel=1&msg={Uri.EscapeDataString(msg)}");
            }

            // ❗❗ KHÔNG LƯU ĐƠN Ở ĐÂY
            // Chỉ hiển thị trang kết quả, đơn đã được tạo ở NotifyUrl

            return Redirect("/Customer/DonDatHang/Index");
        }


        // =============================
        // 3) NOTIFY URL (LƯU ĐƠN HÀNG)
        // =============================
        [HttpPost]
        public IActionResult MomoNotify([FromBody] Dictionary<string, object> data)
        {
            try
            {
                string resultCode = data["resultCode"].ToString();
                if (resultCode != "0")
                    return Ok(); // thất bại → không lưu đơn

                // Lấy orderId MoMo gửi về
                string orderId = data["orderId"].ToString();
                string amount = data["amount"].ToString();

                // Lấy userId từ ExtraData hoặc session tùy bạn muốn
                // Ở đây mình dùng session để biết ai đang thanh toán
                var userId = HttpContext.Session.GetString("UID")
                             ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                    return Ok(); // không xác định user → không lưu đơn

                var cartItems = _unitOfWork.chiTietGioHang.GetAll(
                    u => u.idNguoiDung == userId,
                    includeProperties: "SanPham"
                ).ToList();

                if (!cartItems.Any())
                    return Ok();

                // Tạo đơn
                var donHang = new DonDatHang
                {
                    idDonDat = orderId, // dùng orderId MoMo luôn → UNIQUE 100%
                    idNguoiDung = userId,
                    ngayDat = DateTime.Now,
                    trangThai = "Chờ xác nhận",
                    thanhToan = "Đã thanh toán",
                    ngayThanhToan = DateTime.Now
                };

                _unitOfWork.DonDatHang.Add(donHang);

                // Tạo chi tiết đơn
                foreach (var item in cartItems)
                {
                    var ct = new ChiTietDonHang
                    {
                        idDonDat = orderId,
                        idSanPham = item.idSanPham,
                        soluong = item.soLuongTrongGio,
                        donGia = item.SanPham.gia
                    };

                    _unitOfWork.ChiTietDonHang.Add(ct);
                }

                // Xóa giỏ hàng
                foreach (var item in cartItems)
                    _unitOfWork.chiTietGioHang.Remove(item);

                _unitOfWork.save();
            }
            catch
            {
                // luôn trả về OK để MoMo không retry liên tục
                return Ok();
            }

            return Ok();
        }
    }
}
