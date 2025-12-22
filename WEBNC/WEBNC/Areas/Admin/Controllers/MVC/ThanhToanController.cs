using Microsoft.AspNetCore.Mvc;
using WEBNC.DataAccess.Repository.IRepository;

namespace WEBNC.Areas.Admin.Controllers.MVC
{
    [Area("Admin")]
    public class ThanhToanController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ThanhToanController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var list = _unitOfWork.ThanhToan
                .GetAll(includeProperties: "DonDatHang");

            return View(list);
        }

        public IActionResult Details(int id)
        {
            var obj = _unitOfWork.ThanhToan
                .GetFirstOrDefault(x => x.idThanhToan == id, includeProperties: "DonDatHang");

            if (obj == null)
                return NotFound();

            return View(obj);
        }

        [HttpPost]
        public IActionResult MarkPaid(int id)
        {
            var obj = _unitOfWork.ThanhToan
                .GetFirstOrDefault(x => x.idThanhToan == id);

            if (obj == null)
                return NotFound();

            obj.daThanhToan = true;
            obj.ngayThanhToan = DateTime.Now;

            _unitOfWork.ThanhToan.Update(obj);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}
