using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.DataAccess.Repository;
using WEBNC.Data;
using WEBNC.DataAccess.Repository.IRepository;
using WEBNC.Models;

namespace WEBNC.DataAccess.Repository
{
    public class SanPhamRepository : Repository<SanPham>, ISanPhamRepository
    {
        public readonly ApplicationDbContext _db;
        public SanPhamRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(SanPham obj)
        {
            var objFromDb = _db.SanPham.SingleOrDefault(u => u.idSanPham == obj.idSanPham);
            if (objFromDb != null)
            {
                objFromDb.tenSanPham = obj.tenSanPham;
                objFromDb.idCongTy = obj.idCongTy;
                objFromDb.idLoaiSanPham = obj.idLoaiSanPham;
                if (!string.IsNullOrEmpty(obj.imageURL))
                {
                    objFromDb.imageURL = obj.imageURL;
                }
            }
        }

    }
}
