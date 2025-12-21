using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Security.Claims;
using WEBNC.DataAccess.Repository.IRepository;
using WEBNC.Models;

namespace WEBNC.Areas.Customer.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhGiaController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;
        public DanhGiaController(IUnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _env = env;
        }
        [HttpGet]
        public IActionResult getDanhGia(string idSanPham)
        {
            if (idSanPham == null)
                return BadRequest(new { message = "id sản phẩm không được null!" });
            IEnumerable<DanhGiaSanPham> DanhGiaSanPham = _unitOfWork.DanhGia.GetDanhGiaBySanPham(idSanPham);
            if (DanhGiaSanPham == null|| !DanhGiaSanPham.Any())
                return NotFound("Chưa tồn tại đánh giá nào!");
            var result = new
            {
                diemTrungBinh = Math.Round(DanhGiaSanPham.Average(d => d.soSao), 1),

                danhGiaList = DanhGiaSanPham.Select(d => new
                {
                    userName = d.User?.hoTen ?? d.User?.UserName,
                    soSao = d.soSao,
                    noiDung = d.noiDung,
                    ngayDanhGia = d.ngayDanhGia,
                    hinhAnhs = d.HinhAnhDanhGias != null ? d.HinhAnhDanhGias.Select(h => h.imageUrl).ToList(): new List<string>()
                })
            };
            return Ok(result);
        }
        [HttpPost]
        [Authorize]
        public IActionResult danhGia([FromForm] String idSanPham, [FromForm] string noiDung, [FromForm] int soSao, [FromForm] List<IFormFile>? hinhAnhs)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();
            if (idSanPham == null || soSao < 1 || soSao > 5)
                return BadRequest(new { Message = "Dữ liệu không hợp lệ" });
            if (_unitOfWork.DanhGia.DaDanhGia(idSanPham, userId, null))
                return BadRequest(new { message = "Bạn đã đánh giá sản phẩm này rồi!" });
            DanhGiaSanPham danhGiaSanPham = new DanhGiaSanPham()
            {
                userId=userId,
                idSanPham = idSanPham,
                noiDung = noiDung,
                soSao = soSao,
                ngayDanhGia = DateTime.Now
            };
            _unitOfWork.DanhGia.Add(danhGiaSanPham);
            _unitOfWork.Save();
            if(hinhAnhs!=null && hinhAnhs.Count > 0)
            {
                String uploadPath = Path.Combine(_env.WebRootPath, "images/danhgia");
                if(!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);
                foreach (var hinhAnh in hinhAnhs)
                {
                    String fileName = Guid.NewGuid().ToString() + Path.GetExtension(hinhAnh.FileName);
                    String filePath = Path.Combine(uploadPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        hinhAnh.CopyTo(stream);
                    }
                    HinhAnhDanhGia hinhAnhDanhGia = new HinhAnhDanhGia()
                    {
                        idDanhGia =danhGiaSanPham.idDanhGia,
                        imageUrl = "/images/danhgia/" + fileName
                    };
                    _unitOfWork.HinhAnhDanhGia.Add(hinhAnhDanhGia);
                }
                _unitOfWork.Save();
            }
            return Ok(new {success=true});
        }
    }
}
