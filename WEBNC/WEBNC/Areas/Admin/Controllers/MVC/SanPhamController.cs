using Microsoft.AspNetCore.Mvc;
using WEBNC.DataAccess.Repository.IRepository;

[Area("Admin")]
public class SanPhamController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public SanPhamController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        var list = _unitOfWork.SanPham.GetAll(includeProperties: "LoaiSanPham");

        return View(list);
    }
}
