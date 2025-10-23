using Web.DataAccess.Repository.IRepository;
using WEBNC.Data;

namespace Web.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            //sanpham = new SanPham(_db);
        }
        //public ISanPham sanpham { get; private set; }

        public void save()
        {
            _db.SaveChanges();
        }
    }
}
