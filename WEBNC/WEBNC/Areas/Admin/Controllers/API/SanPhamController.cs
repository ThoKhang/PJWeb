using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEBNC.DataAccess.Repository.IRepository;

namespace WEBNC.Areas.Admin.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanPhamController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public SanPhamController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult GetAll()
        {
            IEnumerable<Models.SanPham> objList = _unitOfWork.SanPham.GetAll(includeProperties: "LoaiSanPham,CongTy");
            return Ok(objList);
        }
    }
}
