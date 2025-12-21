using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WEBNC.DataAccess.Repository.IRepository;
using WEBNC.Models;

namespace WEBNC.Areas.Customer.Controllers.MVC
{
    [Area("Customer")]
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckoutController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account", new { area = "Identity" });
            
            // If user has complete info, redirect to PaymentMethod
            // if (!string.IsNullOrEmpty(user.soNha) && !string.IsNullOrEmpty(user.PhoneNumber))
            // {
            //     return RedirectToAction("PaymentMethod");
            // }

            // Otherwise, show the address input form (or redirect to profile?)
            // For now, let's keep the logic simple as per user request: "remove d:\HK125\C" which implies skipping this step
            // But we should probably redirect to PaymentMethod directly if the intent is to "skip" this page
            
            return RedirectToAction("PaymentMethod");
        }

        [HttpPost]
        public async Task<IActionResult> SaveAddress(ApplicationUser model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account", new { area = "Identity" });

            user.hoTen = model.hoTen;
            user.PhoneNumber = model.PhoneNumber;
            user.soNha = model.soNha;
            user.idPhuongXa = model.idPhuongXa;

            await _userManager.UpdateAsync(user);

            return RedirectToAction("PaymentMethod");
        }

        public async Task<IActionResult> PaymentMethod()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account", new { area = "Identity" });

            // Ensure we load the related entities if needed (e.g., PhuongXa)
            // But ApplicationUser model has idPhuongXa, we might need to load the text for display
            // For now, passing the user object is enough for basic info
            
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmOrder(string paymentMethod)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account", new { area = "Identity" });

            // Handle order confirmation based on payment method
            switch (paymentMethod?.ToLower())
            {
                case "cod":
                    // Process COD order
                    var cartItems = _unitOfWork.chiTietGioHang.GetAll(u => u.idNguoiDung == user.Id, includeProperties: "SanPham");
                    if (!cartItems.Any()) return RedirectToAction("Index", "Cart");

                    var orderId = _unitOfWork.DonDatHang.GenerateNewOrderId();
                    
                    var donHang = new DonDatHang
                    {
                        idDonDat = orderId,
                        idNguoiDung = user.Id,
                        ngayDat = DateTime.Now,
                        trangThai = "Chờ xác nhận",
                        thanhToan = "Tiền mặt",
                        daThanhToan = false,
                        soNha = user.soNha ?? "",
                        sdtGiaoHang = user.PhoneNumber ?? ""
                    };

                    _unitOfWork.DonDatHang.Add(donHang);

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

                    _unitOfWork.chiTietGioHang.RemoveRange(cartItems);
                    _unitOfWork.Save();

                    return RedirectToAction("Index", "DonDatHang");
                
                case "momo":
                    // This will be handled by the PaymentController
                    // We shouldn't get here if JS submits to PaymentController directly
                    // But if we do, redirect to PaymentController creation (via GET - not supported, but fallback)
                    // Or return error
                    return RedirectToAction("CreatePaymentMomo", "Payment", new { area = "Customer" });
                
                case "banking":
                    // Show bank transfer information
                    return View("BankTransfer");
                
                default:
                    return RedirectToAction("PaymentMethod");
            }
        }

        public IActionResult BankTransfer()
        {
            return View();
        }
    }
}
