using Microsoft.AspNetCore.Mvc;
using WEBNC.DataAccess.Repository.IRepository;
using WEBNC.Models;

[Route("api/admin/dondathang")]
[ApiController]
public class DonDatHangApiController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    public DonDatHangApiController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    [HttpGet]
    public IActionResult GetAll()
    {
        var donHang = _unitOfWork.DonDatHang.GetAll(includeProperties: "nguoiDung");
        return Ok(donHang);
    }
    [HttpPost("update-status")]
    public IActionResult UpdateStatus([FromBody] UpdateTrangThaiDonHangDto request)
    {
        if (request == null)
            return BadRequest("Request null");
        var donHang = _unitOfWork.DonDatHang.GetFirstOrDefault(x => x.idDonDat == request.idDonDat);
        if (donHang == null)
            return NotFound("Không tìm thấy đơn hàng");
        donHang.trangThai = request.trangThai;
        if (request.trangThai == "Đã giao")
            donHang.ngayGiaoDuKien = DateTime.Now;
        _unitOfWork.Save();
        return Ok(new { message = "Cập nhật thành công" });
    }
}
