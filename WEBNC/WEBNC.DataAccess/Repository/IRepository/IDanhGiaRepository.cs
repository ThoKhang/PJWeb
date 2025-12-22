using WEBNC.Models;

namespace WEBNC.DataAccess.Repository.IRepository
{
    public interface IDanhGiaRepository : IRepository<DanhGiaSanPham>
    {
        void Update(DanhGiaSanPham obj);
        IEnumerable<DanhGiaSanPham> GetDanhGiaBySanPham(string idSanPham);
        bool DaDanhGia(string idSanPham, string userId, string? idDonDat);
        double TinhDiemTrungBinh(string idSanPham);
    }
}
