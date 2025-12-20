using WEBNC.DataAccess.Repository.IRepository;
using WEBNC.Models;

namespace WEBNC.DataAccess.Repository
{
    public class ThanhToanRepository : Repository<ThanhToan>, IThanhToanRepository
    {
        private readonly ApplicationDbContext _db;
        public ThanhToanRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ThanhToan obj)
        {
            _db.ThanhToan.Update(obj);
        }
    }
}
