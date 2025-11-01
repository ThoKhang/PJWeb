using Web.DataAccess.Repository.IRepository;
using WEBNC.Data;
using WEBNC.DataAccess.Repository;
using WEBNC.DataAccess.Repository.IRepository;

namespace Web.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            SanPham = new SanPhamRepository(_db);
        }
        public ISanPhamRepository SanPham { get; private set; }

        public void save()
        {
            _db.SaveChanges();
        }
    }
}
