using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WEBNC.DataAccess.Repository.IRepository;
using WEBNC.Models;

namespace WEBNC.DataAccess.Repository
{
    public class LoaiSanPhamRepository : Repository<LoaiSanPham>, ILoaiSanPhamRepository
    {
        public readonly ApplicationDbContext _db;
        public LoaiSanPhamRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(LoaiSanPham obj)
        {
            var objFromDb = _db.LoaiSanPham.SingleOrDefault(u => u.idLoaiSanPham == obj.idLoaiSanPham);
            if (objFromDb != null)
                objFromDb.tenLoaiSanPham = obj.tenLoaiSanPham;
        }
    }
}
