using Microsoft.AspNetCore.Mvc;
using WEBNC.DataAccess.Repository.IRepository;

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
        var obj = _unitOfWork.LoaiSanPham.GetFirstOrDefault(x => x.idLoaiSanPham == id);
        if (obj == null)
            return NotFound(new { message = "Không tồn tại" });
        try
        {
            _unitOfWork.LoaiSanPham.Remove(obj);
            _unitOfWork.Save();
            return Ok(new { success = true });
        }
        catch
        {
            return BadRequest(new { message = "Loại sản phẩm đang được sử dụng" });
        }
    }
}
