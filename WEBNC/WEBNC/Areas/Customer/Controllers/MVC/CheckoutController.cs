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
        [HttpPost("/create")]
        public IActionResult Create([FromBody] ThanhToan model)
        {
            if (model == null)
                return BadRequest("Dữ liệu thanh toán không hợp lệ");
            model.idThanhToan = 0;
            model.ngayThanhToan = null;
            model.maGiaoDich = null;
            model.daThanhToan = false;
            if (model.phuongThuc == "MOMO")
            {
                model.daThanhToan = true;
                model.ngayThanhToan = DateTime.Now;
                model.maGiaoDich = Guid.NewGuid().ToString();
            }
            _unitOfWork.ThanhToan.Add(model);
            _unitOfWork.Save();
            return Ok(new
            {
                message = "Tạo thanh toán thành công",
                data = model
            });
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmOrder(string paymentMethod)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "Identity" });

            var cartItems = _unitOfWork.chiTietGioHang
                .GetAll(u => u.idNguoiDung == user.Id, includeProperties: "SanPham");

            if (!cartItems.Any())
                return RedirectToAction("Index", "Cart");

            var orderId = _unitOfWork.DonDatHang.GenerateNewOrderId().Trim();
            var method = paymentMethod?.ToUpper();

            var donHang = new DonDatHang
            {
                idDonDat = orderId,
                idNguoiDung = user.Id,
                ngayDat = DateTime.Now,
                trangThai = "Chờ xác nhận",
                thanhToan = method,
                daThanhToan = false,
                soNha = user.soNha ?? "",
                sdtGiaoHang = user.PhoneNumber ?? ""
            };

            _unitOfWork.DonDatHang.Add(donHang);

            decimal tongTien = 0;

            foreach (var item in cartItems)
            {
                tongTien += item.soLuongTrongGio * item.SanPham.gia;

                _unitOfWork.ChiTietDonHang.Add(new ChiTietDonHang
                {
                    idDonDat = orderId,
                    idSanPham = item.idSanPham,
                    soluong = item.soLuongTrongGio,
                    donGia = item.SanPham.gia
                });
            }

            var thanhToan = new ThanhToan
            {
                idDonDat = orderId,
                phuongThuc = method,
                soTien = tongTien,
                daThanhToan = method == "MOMO",
                ngayThanhToan = DateTime.Now,
                maGiaoDich = null
            };

            _unitOfWork.ThanhToan.Add(thanhToan);
            _unitOfWork.chiTietGioHang.RemoveRange(cartItems);

            _unitOfWork.Save();

            return RedirectToAction("Index", "DonDatHang");
        }


        public IActionResult BankTransfer()
        {
            return View();
        }
    }
}
