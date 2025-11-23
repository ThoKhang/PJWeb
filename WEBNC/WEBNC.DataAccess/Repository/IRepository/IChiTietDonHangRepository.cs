using WEBNC.Models;

namespace WEBNC.DataAccess.Repository.IRepository
{
    public interface IChiTietDonHangRepository : IRepository<ChiTietDonHang>
    {
        void Update(ChiTietDonHang obj);
        IEnumerable<ChiTietDonHang> LayTheoDon(string idDonDat);
    }
}
