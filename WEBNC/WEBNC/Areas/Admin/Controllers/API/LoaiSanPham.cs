using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEBNC.DataAccess.Repository.IRepository;

namespace WEBNC.Areas.Admin.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiSanPham : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public LoaiSanPham(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("loaiSanPham")]
        public IActionResult GetAll()
        {
            IEnumerable<Models.LoaiSanPham> objList = _unitOfWork.LoaiSanPham.GetAll();
            if(objList == null || !objList.Any())
            {
                return NotFound();
            }
            return Ok(objList);
        }
    }
}
