using Microsoft.AspNetCore.Mvc;
using WEBNC.DataAccess.Repository.IRepository;

namespace WEBNC.Areas.Admin.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public SanPhamController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Models.SanPham> objList = _unitOfWork.SanPham.GetAll(includeProperties: "LoaiSanPham,CongTy");
            return View(objList);
        }
    }
}
