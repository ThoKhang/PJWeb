using Web.DataAccess.Repository.IRepository;
using WEBNC.Models;

namespace WEBNC.DataAccess.Repository.IRepository
{
    public interface IDonDatHangRepository : IRepository<DonDatHang>
    {
        void Update(DonDatHang obj);
        IEnumerable<DonDatHang> LayDonTheoNguoiDung(string idNguoiDung);
    }
}
