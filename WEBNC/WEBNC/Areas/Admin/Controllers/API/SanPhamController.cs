using Microsoft.AspNetCore.Mvc;
using WEBNC.DataAccess.Repository.IRepository;

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
    }
}
