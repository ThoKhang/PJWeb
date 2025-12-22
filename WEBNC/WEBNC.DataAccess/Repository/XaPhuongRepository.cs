using WEBNC.DataAccess.Repository.IRepository;
using WEBNC.Models;

namespace WEBNC.DataAccess.Repository
{
    public class XaPhuongRepository : Repository<XaPhuong>, IXaPhuongRepository
    {
        private readonly ApplicationDbContext _db;

        public XaPhuongRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(XaPhuong obj)
        {
            _db.XaPhuong.Update(obj);
        }
    }
}