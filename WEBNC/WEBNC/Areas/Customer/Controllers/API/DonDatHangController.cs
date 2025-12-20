using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public DonDatHangController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        // POST: api/customer/products/{id}
        [HttpPost("~/api/customer/products/{id}")]
        public async Task<IActionResult> BuyNow([FromRoute] string id, [FromBody] JsonElement request)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return BadRequest(new { success = false, message = "ID sản phẩm không hợp lệ." });

                // Lấy số lượng (mặc định 1)
                int quantity = 1;
                if (request.TryGetProperty("soLuong", out JsonElement soLuongElement) &&
                    soLuongElement.TryGetInt32(out int parsedQty) && parsedQty > 0)
                {
                    quantity = parsedQty;
                }

                // Lấy dữ liệu từ modal (nếu có)
                string? newSdt = null;
                if (request.TryGetProperty("sdt", out JsonElement sdtElement))
                    newSdt = sdtElement.GetString()?.Trim();

                string? newSoNha = null;
                if (request.TryGetProperty("soNha", out JsonElement soNhaElement))
                    newSoNha = soNhaElement.GetString()?.Trim();

                string? newIdPhuongXa = null;
                if (request.TryGetProperty("idPhuongXa", out JsonElement idPhuongXaElement))
                    newIdPhuongXa = idPhuongXaElement.GetString()?.Trim();

                var claim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (claim == null)
                    return Unauthorized(new { success = false, message = "Hết phiên đăng nhập." });

                string userId = claim.Value;

                var appUser = await _userManager.FindByIdAsync(userId);
                if (appUser == null)
                    return BadRequest(new { success = false, message = "Không tìm thấy người dùng." });

                var sanPham = _unitOfWork.SanPham.GetFirstOrDefault(u => u.idSanPham == id);
                if (sanPham == null)
                    return BadRequest(new { success = false, message = "Sản phẩm không tồn tại." });

                // Cập nhật thông tin từ modal vào profile
                bool updated = false;
                if (!string.IsNullOrEmpty(newSdt))
                {
                    appUser.PhoneNumber = newSdt;
                    updated = true;
                }
                if (!string.IsNullOrEmpty(newSoNha))
                {
                    appUser.soNha = newSoNha;
                    updated = true;
                }
                if (!string.IsNullOrEmpty(newIdPhuongXa))
                {
                    appUser.idPhuongXa = newIdPhuongXa;
                    updated = true;
                }

                if (updated)
                {
                    await _userManager.UpdateAsync(appUser);
                }

                // KIỂM TRA BẮT BUỘC SĐT VÀ ĐỊA CHỈ
                bool missingPhone = string.IsNullOrEmpty(appUser.PhoneNumber?.Trim());
                bool missingAddress = string.IsNullOrEmpty(appUser.soNha?.Trim()) || string.IsNullOrEmpty(appUser.idPhuongXa);

                if (missingPhone || missingAddress)
                {
                    return Ok(new
                    {
                        success = false,
                        requiresInfo = true,
                        missingPhone = missingPhone,
                        missingAddress = missingAddress,
                        message = "Vui lòng nhập đầy đủ thông tin giao hàng."
                    });
                }

                string sdtGiaoHang = appUser.PhoneNumber.Trim();

                // Ghép địa chỉ đầy đủ
                string diaChiGiaoHang = "Chưa cập nhật địa chỉ";
                string soNhaPart = appUser.soNha?.Trim() ?? "";
                string xaPhuongPart = "";
                string tinhPart = "";

                if (!string.IsNullOrEmpty(appUser.idPhuongXa))
                {
                    var xaPhuong = _unitOfWork.XaPhuong.GetFirstOrDefault(
                        x => x.idXaPhuong == appUser.idPhuongXa,
                        includeProperties: "tinh");

                    if (xaPhuong != null)
                    {
                        xaPhuongPart = xaPhuong.tenXaPhuong?.Trim() ?? "";
                        tinhPart = xaPhuong.tinh?.tenTinh?.Trim() ?? "";
                    }
                }

                var parts = new[] { soNhaPart, xaPhuongPart, tinhPart }
                    .Where(p => !string.IsNullOrEmpty(p))
                    .ToArray();

                diaChiGiaoHang = parts.Length > 0 ? string.Join(", ", parts) : "Chưa cập nhật địa chỉ";

                // Tạo mã đơn hàng
                var existingOrders = _unitOfWork.DonDatHang.GetAll()
                    .Select(d => d.idDonDat)
                    .Where(d => d != null && d.StartsWith("DH") && d.Length > 2 && long.TryParse(d.Substring(2), out _))
                    .ToList();

                string maDonHang = existingOrders.Any()
                    ? "DH" + (existingOrders.Max(o => long.Parse(o.Substring(2))) + 1).ToString("D3")
                    : "DH001";

                // Tạo đơn hàng
                DonDatHang donHang = new DonDatHang
                {
                    idDonDat = maDonHang,
                    idNguoiDung = userId,
                    ngayDat = DateTime.Now,
                    trangThai = "Chờ xác nhận",
                    thanhToan = "Chưa thanh toán",
                    sdtGiaoHang = sdtGiaoHang,
                    soNha = diaChiGiaoHang,
                    ngayGiaoDuKien = DateTime.Now.AddDays(3)
                };

                _unitOfWork.DonDatHang.Add(donHang);

                ChiTietDonHang chiTiet = new ChiTietDonHang
                {
                    idDonDat = donHang.idDonDat,
                    idSanPham = id,
                    soluong = quantity,
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
                var errorMsg = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { success = false, message = "Lỗi Server: " + errorMsg });
            }
        }

        // GET: api/DonDatHang/get-phuongxa - LẤY DANH SÁCH PHƯỜNG/XÃ + TỈNH (KHÔNG CẦN CONTROLLER MỚI)
        [HttpGet("get-phuongxa")]
        public IActionResult GetPhuongXa()
        {
            var list = _unitOfWork.XaPhuong.GetAll(includeProperties: "tinh")
                .Select(x => new
                {
                    id = x.idXaPhuong,
                    text = $"{x.tenXaPhuong}, {x.tinh.tenTinh}"
                })
                .OrderBy(x => x.text)
                .ToList();

            return Ok(list);
        }

        // GET: api/DonDatHang/get-my-orders
        [HttpGet("get-my-orders")]
        public IActionResult GetMyOrders()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null) return Unauthorized(new { message = "Bạn chưa đăng nhập" });

            string userId = claim.Value;
            var donHangs = _unitOfWork.DonDatHang.GetAll(d => d.idNguoiDung == userId, includeProperties: "ChiTietDonHang");

            var result = donHangs.Select(d => new
            {
                idDonDat = d.idDonDat,
                ngayDat = d.ngayDat,
                trangThai = d.trangThai,
                tongTien = d.ChiTietDonHang?.Sum(c => c.soluong * c.donGia) ?? 0
            }).OrderByDescending(x => x.ngayDat);

            return Ok(new { data = result });
        }

        // GET: api/DonDatHang/details/{id}
        [HttpGet("details/{id}")]
        public IActionResult GetOrderDetails(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null) return Unauthorized();

            string userId = claim.Value;
            var donDat = _unitOfWork.DonDatHang.GetFirstOrDefault(d => d.idDonDat == id && d.idNguoiDung == userId);
            if (donDat == null)
                return NotFound(new { success = false, message = "Không tìm thấy đơn hàng" });

            var chiTiet = _unitOfWork.ChiTietDonHang.GetAll(c => c.idDonDat == id, includeProperties: "SanPham");
            var chiTietResult = chiTiet.Select(c => new
            {
                tenSanPham = c.SanPham?.tenSanPham ?? "Sản phẩm lỗi",
                hinhAnh = c.SanPham?.imageURL ?? "",
                soLuong = c.soluong,
                donGia = c.donGia,
                thanhTien = c.soluong * c.donGia
            });

            return Ok(new
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

        // POST: api/DonDatHang/cancel/{id}
        [HttpPost("cancel/{id}")]
        public IActionResult CancelOrder(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return BadRequest(new { success = false, message = "ID không hợp lệ" });

                var claim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (claim == null)
                    return Unauthorized(new { success = false, message = "Bạn chưa đăng nhập" });

                string userId = claim.Value;
                var donHang = _unitOfWork.DonDatHang.GetFirstOrDefault(u => u.idDonDat == id && u.idNguoiDung == userId);

                if (donHang == null)
                    return NotFound(new { success = false, message = "Không tìm thấy đơn hàng." });

                if (donHang.trangThai != "Chờ xác nhận")
                    return BadRequest(new { success = false, message = "Đơn hàng đã được xử lý hoặc đang giao, không thể hủy." });

                donHang.trangThai = "Đã hủy";
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