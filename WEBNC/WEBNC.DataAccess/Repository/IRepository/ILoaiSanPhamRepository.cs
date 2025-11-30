using WEBNC.Models;

namespace WEBNC.DataAccess.Repository.IRepository
{
    public interface ILoaiSanPhamRepository : IRepository<LoaiSanPham>
    {
        void Update(LoaiSanPham obj);
    }
}
