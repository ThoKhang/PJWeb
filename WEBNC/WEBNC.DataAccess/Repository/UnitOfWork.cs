using Web.DataAccess.Repository.IRepository;
using WEBNC.Data;
using WEBNC.DataAccess.Repository;
using WEBNC.DataAccess.Repository.IRepository;
using WEBNC.Models;

namespace Web.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            SanPham = new SanPhamRepository(_db);
            DonDatHang = new DonDatHangRepository(_db);
            ChiTietDonHang = new ChiTietDonHangRepository(_db);

        }
        public ISanPhamRepository SanPham { get; private set; }
        public IDonDatHangRepository DonDatHang { get; private set; }
        public IChiTietDonHangRepository ChiTietDonHang { get; private set; }
        public void save()
        {
            _db.SaveChanges();
        }
    }
}
