using WEBNC.Data;
using WEBNC.DataAccess.Repository.IRepository;

namespace Web.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ISanPhamRepository SanPham { get; }
        void save();
    }
}
