using WEBNC.Models;

namespace WEBNC.DataAccess.Repository.IRepository
{
    public interface IThanhToanRepository : IRepository<ThanhToan>
    {
        void Update(ThanhToan obj);
    }
}
