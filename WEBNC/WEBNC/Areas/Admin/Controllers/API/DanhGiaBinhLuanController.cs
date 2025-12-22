using Microsoft.AspNetCore.Mvc;
using WEBNC.DataAccess.Repository.IRepository;

namespace WEBNC.Areas.Admin.Controllers.API
{
    [Route("api/admin/danhgia")]
    [ApiController]
    public class DanhGiaBinhLuanController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public DanhGiaBinhLuanController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var danhGia = _unitOfWork.DanhGia.GetAll(includeProperties: "SanPham,User,DonDatHang");
            return Ok(danhGia);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var dg = _unitOfWork.DanhGia.GetFirstOrDefault(x => x.idDanhGia == id);
            if (dg == null)
                return NotFound(new { message = "Không tìm thấy đánh giá" });
            _unitOfWork.DanhGia.Remove(dg);
            _unitOfWork.Save();
            return Ok(new { message = "Đã xoá đánh giá" });
        }
    }
}
