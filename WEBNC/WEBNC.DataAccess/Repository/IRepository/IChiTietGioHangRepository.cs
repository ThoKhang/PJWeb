using WEBNC.Models;

namespace WEBNC.DataAccess.Repository.IRepository
{
    public interface IChiTietGioHangRepository: IRepository<ChiTietGioHang>
    {
        void Update(ChiTietGioHang obj);
        int IncrementCount(ChiTietGioHang objd, int count);
        int DecrementCount(ChiTietGioHang obj, int count);
    }
}
