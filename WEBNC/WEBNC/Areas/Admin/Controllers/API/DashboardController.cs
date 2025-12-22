using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WEBNC.Areas.Admin.Controllers.API
{
    [Area("Admin")]
    [Route("api/admin/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public DashboardController(ApplicationDbContext db)
        {
            _db = db;
        }

        // ========== 1. SUMMARY CARD (trên cùng) ==========
        // - Doanh thu hôm nay (theo thanh toán đã thanh toán)
        // - Số đơn hôm nay
        // - Số khách có đơn trong 7 ngày gần đây
        // - Số sản phẩm còn hàng
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var today = DateTime.Today;
            var sevenDaysAgo = today.AddDays(-7);

            // Doanh thu hôm nay: từ bảng ThanhToan, đã thanh toán, theo ngàyThanhToan
            var todayRevenue = await _db.ThanhToan
                .Where(t =>
                    t.daThanhToan &&
                    t.ngayThanhToan.HasValue &&
                    t.ngayThanhToan.Value.Date == today)
                .SumAsync(t => (decimal?)t.soTien) ?? 0;

            // Số đơn hôm nay: theo ngày đặt đơn
            var todayOrders = await _db.DonDatHang
                .CountAsync(d =>
                    d.ngayDat.HasValue &&
                    d.ngayDat.Value.Date == today);

            // Số khách có đơn trong 7 ngày gần đây
            var newUsers7Days = await _db.DonDatHang
                .Where(d =>
                    d.ngayDat.HasValue &&
                    d.ngayDat.Value >= sevenDaysAgo)
                .Select(d => d.idNguoiDung)
                .Distinct()
                .CountAsync();

            // Số sản phẩm còn hàng
            var totalActiveProducts = await _db.SanPham
                .CountAsync(p => p.soLuongHienCon > 0);

            return Ok(new
            {
                todayRevenue,
                todayOrders,
                newUsers7Days,
                totalActiveProducts
            });
        }

        // ========== 2. DOANH THU THEO THÁNG ==========
        // Lấy theo bảng ThanhToan, đã thanh toán, trong năm hiện tại
        [HttpGet("revenue-by-month")]
        public async Task<IActionResult> GetRevenueByMonth()
        {
            var year = DateTime.Today.Year;

            var query = await _db.ThanhToan
                .Where(t =>
                    t.daThanhToan &&
                    t.ngayThanhToan.HasValue &&
                    t.ngayThanhToan.Value.Year == year)
                .GroupBy(t => t.ngayThanhToan!.Value.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Total = g.Sum(x => x.soTien)
                })
                .ToListAsync();

            // build đủ 12 tháng
            var labels = Enumerable.Range(1, 12)
                .Select(m => $"Th{m}")
                .ToArray();

            var values = Enumerable.Range(1, 12)
                .Select(m =>
                    query.FirstOrDefault(x => x.Month == m)?.Total ?? 0)
                .ToArray();

            return Ok(new { labels, values });
        }

        // ========== 3. TỈ LỆ ĐƠN THEO TRẠNG THÁI ==========
        // Dựa trên DonDatHang.trangThai trong 30 ngày gần đây
        [HttpGet("order-status-ratio")]
        public async Task<IActionResult> GetOrderStatusRatio()
        {
            var from = DateTime.Today.AddDays(-30);

            var data = await _db.DonDatHang
                .Where(d =>
                    d.ngayDat.HasValue &&
                    d.ngayDat.Value >= from)
                .GroupBy(d => d.trangThai)
                .Select(g => new
                {
                    status = g.Key,
                    count = g.Count()
                })
                .ToListAsync();

            return Ok(data);
        }

        // ========== 4. ĐƠN HÀNG MỚI GẦN ĐÂY ==========
        // 5 đơn gần nhất + tổng tiền từng đơn (tính từ ChiTietDonHang)
        [HttpGet("recent-orders")]
        public async Task<IActionResult> GetRecentOrders()
        {
            var data = await _db.DonDatHang
                .OrderByDescending(d => d.ngayDat)
                .Take(5)
                .Select(d => new
                {
                    id = d.idDonDat,
                    code = d.idDonDat, // mã đơn, dùng luôn idDonDat
                    customer = d.nguoiDung != null
                        ? d.nguoiDung.UserName
                        : d.soNha, // nếu không có tên user thì hiện địa chỉ
                    date = d.ngayDat,
                    total = d.ChiTietDonHang
                        .Sum(ct => ct.soluong * ct.donGia),
                    status = d.trangThai
                })
                .ToListAsync();

            return Ok(data);
        }
    }
}
