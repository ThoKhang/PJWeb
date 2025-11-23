using Microsoft.EntityFrameworkCore;
using WEBNC.Data;
using WEBNC.DataAccess.Repository.IRepository;
using WEBNC.Models;

namespace WEBNC.DataAccess.Repository
{
    public class ChiTietDonHangRepository : Repository<ChiTietDonHang>, IChiTietDonHangRepository
    {
        private readonly ApplicationDbContext _db;

        public ChiTietDonHangRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ChiTietDonHang obj)
        {
            var objFromDb = _db.ChiTietDonHang
                .FirstOrDefault(c => c.idDonDat == obj.idDonDat && c.idSanPham == obj.idSanPham);

            if (objFromDb != null)
            {
                objFromDb.soluong = obj.soluong;
                objFromDb.donGia = obj.donGia;
            }
        }

        public IEnumerable<ChiTietDonHang> LayTheoDon(string idDonDat)
        {
            return _db.ChiTietDonHang
                .Where(ct => ct.idDonDat == idDonDat)
                .Include(ct => ct.SanPham)
                .ToList();
        }
    }
}
