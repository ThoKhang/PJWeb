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
        return View();
    }
    public IActionResult Upsert(string? id)
    {
        ViewBag.Id = id;
        return View();
    }
}
