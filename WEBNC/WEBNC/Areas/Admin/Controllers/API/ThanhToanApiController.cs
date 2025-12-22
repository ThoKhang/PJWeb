using Microsoft.AspNetCore.Mvc;
using WEBNC.DataAccess.Repository.IRepository;
using WEBNC.Models;

namespace WEBNC.Areas.Admin.Controllers.API
{
    [Area("Admin")]
    [Route("api/admin/thanhtoan")]
    [ApiController]
    public class ThanhToanApiController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ThanhToanApiController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/admin/thanhtoan
        [HttpGet]
        public IActionResult GetAll()
        {
            var objList = _unitOfWork.ThanhToan.GetAll();

            // ❌ Đừng trả NotFound khi không có dữ liệu
            if (objList == null || !objList.Any())
                return NotFound();

            // ✅ Luôn trả về 200 với list (kể cả rỗng)
            return Ok(objList);
        }


        // GET: api/admin/thanhtoan/5
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var obj = _unitOfWork.ThanhToan
                .GetFirstOrDefault(x => x.idThanhToan == id, includeProperties: "DonDatHang");

            if (obj == null)
                return NotFound(new { message = "Thanh toán không tồn tại" });

            return Ok(obj);
        }

        // POST: api/admin/thanhtoan
        [HttpPost]
        public IActionResult Create([FromBody] ThanhToan model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _unitOfWork.ThanhToan.Add(model);
            _unitOfWork.Save();

            return Ok(new { success = true, data = model });
        }

        // PUT: api/admin/thanhtoan/5
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] ThanhToan model)
        {
            if (id != model.idThanhToan)
                return BadRequest(new { message = "Id không khớp" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var objFromDb = _unitOfWork.ThanhToan
                .GetFirstOrDefault(x => x.idThanhToan == id);

            if (objFromDb == null)
                return NotFound(new { message = "Thanh toán không tồn tại" });

            objFromDb.phuongThuc = model.phuongThuc;
            objFromDb.soTien = model.soTien;
            objFromDb.daThanhToan = model.daThanhToan;
            objFromDb.ngayThanhToan = model.ngayThanhToan;
            objFromDb.maGiaoDich = model.maGiaoDich;
            // nếu bạn có idDonDat thì cập nhật thêm ở đây

            _unitOfWork.ThanhToan.Update(objFromDb);
            _unitOfWork.Save();

            return Ok(new { success = true });
        }

        // DELETE: api/admin/thanhtoan/5
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var obj = _unitOfWork.ThanhToan
                .GetFirstOrDefault(x => x.idThanhToan == id);

            if (obj == null)
                return NotFound(new { message = "Thanh toán không tồn tại" });

            try
            {
                _unitOfWork.ThanhToan.Remove(obj);
                _unitOfWork.Save();
                return Ok(new { success = true });
            }
            catch
            {
                return BadRequest(new { message = "Không thể xóa thanh toán này" });
            }
        }

        // PATCH: api/admin/thanhtoan/5/mark-paid
        [HttpPatch("{id:int}/mark-paid")]
        public IActionResult MarkPaid(int id)
        {
            var obj = _unitOfWork.ThanhToan
                .GetFirstOrDefault(x => x.idThanhToan == id);

            if (obj == null)
                return NotFound(new { message = "Thanh toán không tồn tại" });

            obj.daThanhToan = true;
            obj.ngayThanhToan = DateTime.Now;

            _unitOfWork.ThanhToan.Update(obj);
            _unitOfWork.Save();

            return Ok(new { success = true });
        }
    }
}
