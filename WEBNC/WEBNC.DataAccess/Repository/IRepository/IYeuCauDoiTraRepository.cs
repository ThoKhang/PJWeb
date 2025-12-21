using WEBNC.Models;

namespace WEBNC.DataAccess.Repository.IRepository
{
    public interface IYeuCauDoiTraRepository : IRepository<YeuCauDoiTra>
    {
        void Update(YeuCauDoiTra obj);
    }
}