using Microsoft.AspNetCore.Mvc;
using WEBNC.DataAccess.Repository.IRepository;

namespace WEBNC.Areas.Customer.Controllers.API
{
    [Route("api/products/")]
    [ApiController]
    public class LoaiSanPhamController: ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoaiSanPhamController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("loai")]
        public IActionResult loaiSanPham()
        {
            var listLoaiSP = _unitOfWork.LoaiSanPham.GetAll();
            return Ok(new { data = listLoaiSP });
        }
    }
}
