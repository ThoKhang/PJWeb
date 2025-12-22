using Microsoft.AspNetCore.Mvc;
using WEBNC.DataAccess.Repository.IRepository;
using WEBNC.Models;

[Area("Admin")]
[Route("api/admin/loaisanpham")]
[ApiController]
public class LoaiSanPhamController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public LoaiSanPhamController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var objList = _unitOfWork.LoaiSanPham.GetAll();
        if (objList == null || !objList.Any())
            return NotFound();
        return Ok(objList);
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var loai = _unitOfWork.LoaiSanPham
            .GetFirstOrDefault(x => x.idLoaiSanPham == id);
        if (loai == null)
            return NotFound(new { message = "Không tồn tại" });
        bool dangDuocSuDung = _unitOfWork.SanPham.GetAll(x => x.idLoaiSanPham == id).Any();
        if (dangDuocSuDung)
            return BadRequest(new{message = "Không thể xóa vì loại sản phẩm đang được sử dụng"});
        _unitOfWork.LoaiSanPham.Remove(loai);
        _unitOfWork.Save();
        return Ok(new { success = true });
    }
    [HttpPost]
    public IActionResult CreateOrUpdate([FromBody] LoaiSanPham model)
    {
        if (model == null)
            return BadRequest();
        var obj = _unitOfWork.LoaiSanPham.GetFirstOrDefault(x => x.idLoaiSanPham == model.idLoaiSanPham);
        if (obj != null)
        {
            obj.tenLoaiSanPham = model.tenLoaiSanPham;
            _unitOfWork.Save();
            return Ok(new { success = true, mode = "update" });
        }
        _unitOfWork.LoaiSanPham.Add(model);
        _unitOfWork.Save();
        return Ok(new { success = true, mode = "create" });
    }
}
