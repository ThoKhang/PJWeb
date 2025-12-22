
using WEBNC.DataAccess.Repository.IRepository;
using WEBNC.Models;

namespace WEBNC.DataAccess.Repository
{
    public class YeuCauDoiTraRepository : Repository<YeuCauDoiTra>, IYeuCauDoiTraRepository
    {
        private readonly ApplicationDbContext _db;

        public YeuCauDoiTraRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(YeuCauDoiTra obj)
        {
            _db.YeuCauDoiTra.Update(obj);
        }
    }
}