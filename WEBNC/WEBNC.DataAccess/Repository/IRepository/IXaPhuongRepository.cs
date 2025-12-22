using WEBNC.Models;

namespace WEBNC.DataAccess.Repository.IRepository
{
    public interface IXaPhuongRepository : IRepository<XaPhuong>
    {
        void Update(XaPhuong obj);
    }
}