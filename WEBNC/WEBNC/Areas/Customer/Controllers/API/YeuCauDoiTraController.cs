using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WEBNC.Models;

namespace WEBNC.Areas.Customer.Controllers.API 
{
    
    [Route("api/YeuCauDoiTra")]
    [ApiController]
    
    public class YeuCauDoiTraController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public YeuCauDoiTraController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost] 
        public async Task<IActionResult> Post([FromForm] string idDonDat, [FromForm] string lyDo)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "Vui lòng đăng nhập." });
                }

                if (string.IsNullOrEmpty(lyDo))
                {
                    return BadRequest(new { message = "Vui lòng nhập lý do." });
                }

                // Kiểm tra trùng lặp
                var exists = await _context.YeuCauDoiTra // Lưu ý: check tên bảng trong DBContext (có 's' hay không)
                    .AnyAsync(x => x.idDonDat == idDonDat && x.userId == userId);

                if (exists)
                {
                    return BadRequest(new { message = "Bạn đã gửi yêu cầu cho đơn này rồi." });
                }

                var yeuCau = new YeuCauDoiTra
                {
                    idDonDat = idDonDat,
                    userId = userId,
                    lyDo = lyDo,
                    trangThai = "ChoDuyet",
                    ngayTao = DateTime.Now
                };

                _context.YeuCauDoiTra.Add(yeuCau); // Lưu ý tên bảng
                await _context.SaveChangesAsync();

                return Ok(new { message = "Gửi yêu cầu thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server: " + ex.Message });
            }
        }

        // Lấy ra danh sách yêu cầu đổi trả
        [HttpGet("get-my-requests")]
        public async Task<IActionResult> GetMyRequests()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId)) return Unauthorized();

                var list = await _context.YeuCauDoiTra
                    .Where(x => x.userId == userId)
                    .OrderByDescending(x => x.ngayTao)
                    .Select(x => new
                    {
                        x.idYeuCau,
                        x.idDonDat,
                        x.ngayTao,
                        x.lyDo,
                        x.trangThai
                    })
                    .ToListAsync();

                return Ok(new { data = list });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // Hủy yêu cầu đổi trả (chỉ thực hiện được khi trong trạng thái chờ duyệt)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId)) return Unauthorized();

                // 1. Tìm yêu cầu theo ID và UserID (bảo mật: chỉ xóa của chính mình)
                var yeuCau = await _context.YeuCauDoiTra
                    .FirstOrDefaultAsync(x => x.idYeuCau == id && x.userId == userId);

                if (yeuCau == null)
                {
                    return NotFound(new { message = "Không tìm thấy yêu cầu." });
                }

                // 2. Chỉ cho phép hủy khi trạng thái là "ChoDuyet"
                var validStatuses = new[] { "ChoDuyet", "Chờ duyệt" };

                if (!validStatuses.Contains(yeuCau.trangThai))
                {
                    return BadRequest(new { message = "Không thể hủy yêu cầu đã được xử lý." });
                }

                // 3. Xóa và lưu
                _context.YeuCauDoiTra.Remove(yeuCau);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Đã hủy yêu cầu thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server: " + ex.Message });
            }
        }
    }
}