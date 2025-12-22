using Microsoft.AspNetCore.Mvc;
using WEBNC.DataAccess.Repository.IRepository;
using WEBNC.Models;

namespace WEBNC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("api/admin/sanpham")]
    [ApiController]
    public class SanPhamApiController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SanPhamApiController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var list = _unitOfWork.SanPham
                    // ⚠️ SỬA HOA–THƯỜNG ĐÚNG MODEL
                    .GetAll(includeProperties: "LoaiSanPham,congTy");

                return Ok(list);
            }
            catch (Exception ex)
            {
                // Trả text lỗi để dễ debug (đỡ bị JSON parse fail)
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var sp = _unitOfWork.SanPham.GetFirstOrDefault(
                x => x.idSanPham == id,
                includeProperties: "LoaiSanPham,congTy"
            );
            if (sp == null)
                return NotFound();
            return Ok(sp);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var obj = _unitOfWork.SanPham
                .GetFirstOrDefault(x => x.idSanPham == id);

            if (obj == null)
                return NotFound(new { message = "Không tồn tại sản phẩm" });

            try
            {
                _unitOfWork.SanPham.Remove(obj);
                _unitOfWork.Save();
                return Ok(new { success = true });
            }
            catch
            {
                return BadRequest(new { message = "Sản phẩm đang được sử dụng" });
            }
        }
        [HttpPost("upsert")]
        public IActionResult Upsert([FromForm] SanPham model, IFormFile? image)
        {
            if (model == null)
                return BadRequest();
            var sp = _unitOfWork.SanPham
                .GetFirstOrDefault(x => x.idSanPham == model.idSanPham);
            if (image != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                var path = Path.Combine("wwwroot/images/sanpham", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    image.CopyTo(stream);
                }

                model.imageURL = "/images/sanpham/" + fileName;
            }

            if (sp == null)
            {
                _unitOfWork.SanPham.Add(model);
            }
            else
            {
                sp.tenSanPham = model.tenSanPham;
                sp.gia = model.gia;
                sp.soLuongHienCon = model.soLuongHienCon;
                sp.idLoaiSanPham = model.idLoaiSanPham;
                sp.idCongTy = model.idCongTy;

                if (!string.IsNullOrEmpty(model.imageURL))
                    sp.imageURL = model.imageURL;
            }

            _unitOfWork.Save();
            return Ok(new { success = true });
        }

    }
}
