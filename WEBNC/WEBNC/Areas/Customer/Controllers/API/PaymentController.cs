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

            var returnUrl = baseUrl + "/Customer/Payment/MomoPaymentExecute"; // Redirect về đây sau khi thanh toán
            var notifyUrl = baseUrl + "/Customer/Payment/MomoNotify";        // Momo gọi về đây (IPN)

            // Gửi userId vào OrderInfo để nhận lại qua ExtraData
            // Lưu ý: OrderInfo cần ngắn gọn, không dấu tiếng Việt càng tốt để tránh lỗi encoding
            model.OrderInfo = userId; 

            var response = await _momoService.CreatePaymentMomo(model, returnUrl, notifyUrl);

            if (response == null || string.IsNullOrEmpty(response.PayUrl))
                return BadRequest(new { success = false, message = "Không nhận được PayUrl từ Momo" });

            return Redirect(response.PayUrl);
        }


        // =============================
        // 2) RETURN URL (REDIRECT TỪ BROWSER)
        // =============================
        [HttpGet]
        public IActionResult MomoPaymentExecute()
        {
            var momo = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
            bool isSuccess = momo.ErrorCode == "0" || momo.ResultCode == "0";

            if (!isSuccess)
            {
                var msg = momo.LocalMessage ?? momo.Message ?? "Giao dịch đã hủy";
                return Redirect($"/Customer/Cart/Index?cancel=1&msg={Uri.EscapeDataString(msg)}");
            }

            // Lấy thông tin thanh toán thành công
            string momoOrderId = momo.OrderId;
            string amountStr = momo.Amount;
            string extraData = momo.ExtraData; // Chứa userId

            // Cố gắng lấy userId từ extraData, nếu không có thì lấy từ Session/User hiện tại
            var userId = !string.IsNullOrEmpty(extraData) ? extraData :
                         (HttpContext.Session.GetString("UID") ?? User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(momoOrderId))
            {
                decimal amount = 0;
                decimal.TryParse(amountStr, out amount);
                
                // Gọi hàm xử lý đơn hàng (để hỗ trợ test trên localhost khi Notify không chạy được)
                ProcessOrder(userId, momoOrderId, amount);
            }

            return Redirect("/Customer/DonDatHang/Index");
        }


        // =============================
        // 3) NOTIFY URL (SERVER GỌI SERVER - IPN)
        // =============================
        [HttpPost]
        public IActionResult MomoNotify([FromBody] Dictionary<string, object> data)
        {
            Console.WriteLine("=== START MomoNotify ===");
            try
            {
                if (data == null) return Ok();

                string resultCode = data.ContainsKey("resultCode") ? data["resultCode"].ToString() : "";
                if (resultCode != "0")
                {
                    Console.WriteLine($"MomoNotify: resultCode = {resultCode} (Failed)");
                    return Ok();
                }

                string momoOrderId = data.ContainsKey("orderId") ? data["orderId"].ToString() : "";
                string amountStr = data.ContainsKey("amount") ? data["amount"].ToString() : "0";
                string extraData = data.ContainsKey("extraData") ? data["extraData"].ToString() : "";

                var userId = !string.IsNullOrEmpty(extraData) ? extraData :
                             (HttpContext.Session.GetString("UID") ?? User.FindFirstValue(ClaimTypes.NameIdentifier));

                Console.WriteLine($"MomoNotify: userId resolved = {userId}");

                if (string.IsNullOrEmpty(userId)) return Ok();

                decimal amount = 0;
                decimal.TryParse(amountStr, out amount);

                ProcessOrder(userId, momoOrderId, amount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MomoNotify ERROR: {ex.Message}");
            }
            
            Console.WriteLine("=== END MomoNotify ===");
            return Ok();
        }

        // =============================
        // HÀM XỬ LÝ TẠO ĐƠN HÀNG (DÙNG CHUNG)
        // =============================
        private void ProcessOrder(string userId, string momoOrderId, decimal amount)
        {
            try
            {
                // 1. Kiểm tra đơn hàng đã tồn tại chưa (tránh duplicate khi cả Notify và Return đều chạy)
                var existingPayment = _unitOfWork.ThanhToan.GetFirstOrDefault(t => t.maGiaoDich == momoOrderId);
                if (existingPayment != null)
                {
                    Console.WriteLine($"ProcessOrder: Order with TransactionId {momoOrderId} already exists. Skipping.");
                    return;
                }

                // 2. Lấy giỏ hàng
                var cartItems = _unitOfWork.chiTietGioHang.GetAll(
                    u => u.idNguoiDung == userId,
                    includeProperties: "SanPham"
                ).ToList();

                if (!cartItems.Any())
                {
                    Console.WriteLine("ProcessOrder: Cart is empty, cannot create order.");
                    return;
                }

                // 3. Tạo ID đơn hàng mới
                string newDonDatId = _unitOfWork.DonDatHang.GenerateNewOrderId();
                Console.WriteLine($"ProcessOrder: Generated new OrderId = {newDonDatId}");

                // 4. Tạo đối tượng DonDatHang
                var donHang = new DonDatHang
                {
                    idDonDat = newDonDatId,
                    idNguoiDung = userId,
                    ngayDat = DateTime.Now,
                    trangThai = "Chờ xác nhận",
                    thanhToan = "Đã thanh toán",
                    ngayThanhToan = DateTime.Now,
                    sdtGiaoHang = "00000000000", 
                    soNha = "Chưa cập nhật"        // TODO: Lấy từ user info
                };
                if (_unitOfWork == null) { Console.WriteLine("ProcessOrder: _unitOfWork is null"); return; }
                if (_unitOfWork.DonDatHang == null) { Console.WriteLine("ProcessOrder: _unitOfWork.DonDatHang is null"); return; }
                if (donHang == null) { Console.WriteLine("ProcessOrder: donHang is null"); return; }
                _unitOfWork.DonDatHang.Add(donHang);

                // 5. Tạo đối tượng ThanhToan
                var thanhToanEntity = new ThanhToan
                {
                    idDonDat = newDonDatId,
                    phuongThuc = "Momo",
                    soTien = amount,
                    daThanhToan = true,
                    ngayThanhToan = DateTime.Now,
                    maGiaoDich = momoOrderId
                };
                if (_unitOfWork.ThanhToan == null) { Console.WriteLine("ProcessOrder: _unitOfWork.ThanhToan is null"); return; }
                _unitOfWork.ThanhToan.Add(thanhToanEntity);

                // 6. Tạo ChiTietDonHang
                foreach (var item in cartItems)
                {
                    var ct = new ChiTietDonHang
                    {
                        idDonDat = newDonDatId,
                        idSanPham = item.idSanPham,
                        soluong = item.soLuongTrongGio,
                        donGia = item.SanPham.gia
                    };
                    _unitOfWork.ChiTietDonHang.Add(ct);
                }

                // 7. Xóa giỏ hàng
                foreach (var item in cartItems)
                    _unitOfWork.chiTietGioHang.Remove(item);

                // 8. Lưu tất cả vào DB
                _unitOfWork.save();
                Console.WriteLine("ProcessOrder: SAVED SUCCESSFULLY.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ProcessOrder ERROR: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
        }
    }
}
