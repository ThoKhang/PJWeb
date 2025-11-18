using Microsoft.AspNetCore.Mvc;
using Web.DataAccess.Repository.IRepository;
using WEBNC.Models;
using Microsoft.AspNetCore.Http; // Dùng cho Session
using System.Linq; // Dùng cho Select, Sum

namespace WEBNC.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class DonDatHangController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        // ID người dùng giả định để test (Khớp với User ND001 trong SQL của bạn)
        private const string TEST_USER_ID = "ND001";

        public DonDatHangController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // --- HÀM HỖ TRỢ (HELPER) ---
        // Tự động tạo Session nếu chưa có (Giúp bạn test mà không cần đăng nhập lại)
        private string EnsureSessionExists()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                userId = TEST_USER_ID;
                HttpContext.Session.SetString("UserId", userId);
            }
            return userId;
        }

        // ==========================================
        // PHẦN 1: CÁC ACTION TRẢ VỀ VIEW (GIAO DIỆN HTML)
        // ==========================================

        public IActionResult Index()
        {
            // Chỉ trả về khung HTML, dữ liệu sẽ do JS gọi API GetMyOrders
            EnsureSessionExists();
            return View();
        }

        public IActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            EnsureSessionExists();

            // Truyền ID đơn hàng sang View để Javascript sử dụng
            ViewBag.OrderId = id;
            return View();
        }

        // ==========================================
        // PHẦN 2: CÁC ACTION API (TRẢ VỀ JSON CHO JS GỌI)
        // ==========================================

        [HttpGet]
        public IActionResult GetMyOrders()
        {
            var userId = EnsureSessionExists();

            // 1. Lấy danh sách từ DB
            var donHangs = _unitOfWork.DonDatHang
        .GetAll(d => d.idNguoiDung == userId, includeProperties: "ChiTietDonHang");

            // 2. CHỌN LỌC DỮ LIỆU (Projection)
            // Bước này cực kỳ quan trọng để:
            // - Tránh lỗi vòng lặp (Circular Reference)
            // - Map đúng tên cột SQL (trangThai)
            var result = donHangs.Select(d => new {
                idDonDat = d.idDonDat,
                ngayDat = d.ngayDat,
                trangThai = d.trangThai, // Khớp với cột trong SQL của bạn
                // Tính tổng tiền nếu DB không có cột TongTien riêng
                tongTien = d.ChiTietDonHang != null ? d.ChiTietDonHang.Sum(c => c.soluong * c.donGia) : 0
            });

            return Json(new { data = result });
        }

        [HttpGet]
        public IActionResult GetOrderDetails(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var userId = EnsureSessionExists();

            // 1. Lấy thông tin đơn hàng
            var donDat = _unitOfWork.DonDatHang.GetFirstOrDefault(
        d => d.idDonDat == id && d.idNguoiDung == userId
      );

            if (donDat == null)
            {
                return NotFound(new { success = false, message = "Không tìm thấy đơn hàng" });
            }

            // 2. Lấy chi tiết sản phẩm (Kèm thông tin SanPham để lấy ảnh, tên)
            var chiTiet = _unitOfWork.ChiTietDonHang.GetAll(
        c => c.idDonDat == id,
        includeProperties: "SanPham"
      );

            // 3. Chọn lọc dữ liệu chi tiết để trả về JSON gọn nhẹ
            var chiTietResult = chiTiet.Select(c => new {
                tenSanPham = c.SanPham != null ? c.SanPham.tenSanPham : "Sản phẩm lỗi",
                hinhAnh = c.SanPham != null ? c.SanPham.imageURL : "",
                soLuong = c.soluong,
                donGia = c.donGia,
                thanhTien = c.soluong * c.donGia
            });

            // 4. Trả về cục dữ liệu tổng hợp
            return Json(new
            {
                success = true,
                orderInfo = new
                {
                    idDonDat = donDat.idDonDat,
                    ngayDat = donDat.ngayDat,
                    trangThai = donDat.trangThai,
                    diaChi = donDat.soNha,
                    sdt = donDat.sdtGiaoHang,
                    tongTien = chiTietResult.Sum(x => x.thanhTien)
                },
                orderDetails = chiTietResult
            });
        }
    }
}