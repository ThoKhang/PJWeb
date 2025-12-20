using Microsoft.EntityFrameworkCore;
using WEBNC.DataAccess.Repository.IRepository;
using WEBNC.Models;

namespace WEBNC.DataAccess.Repository
{
    public class DonDatHangRepository : Repository<DonDatHang>, IDonDatHangRepository
    {
        private readonly ApplicationDbContext _db;

        public DonDatHangRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(DonDatHang obj)
        {
            var objFromDb = _db.DonDatHang.FirstOrDefault(u => u.idDonDat == obj.idDonDat);

            if (objFromDb != null)
            {
                objFromDb.sdtGiaoHang = obj.sdtGiaoHang;
                objFromDb.soNha = obj.soNha;
                objFromDb.trangThai = obj.trangThai;
                objFromDb.thanhToan = obj.thanhToan;
                objFromDb.ngayDat = obj.ngayDat;
                objFromDb.ngayThanhToan = obj.ngayThanhToan;
                objFromDb.ngayGiaoDuKien = obj.ngayGiaoDuKien;
            }
        }

        public IEnumerable<DonDatHang> LayDonTheoNguoiDung(string idNguoiDung)
        {
            return _db.DonDatHang
                .Where(d => d.idNguoiDung == idNguoiDung)
                .Include(d => d.ChiTietDonHang)
                .ThenInclude(ct => ct.SanPham)
                .ToList();
        }

        public string GenerateNewOrderId()
        {
            // Lấy tất cả idDonDat để xử lý logic tăng tự động
            // Vì id dạng chuỗi "DHxxx" nên cần xử lý client-side hoặc logic phức tạp
            var ids = _db.DonDatHang.Select(d => d.idDonDat).ToList();
            
            // Logic tìm max
            var maxId = ids
                .Where(id => id.StartsWith("DH"))
                .Select(id =>
                {
                    if (int.TryParse(id.Substring(2), out int num)) return num;
                    return 0;
                })
                .DefaultIfEmpty(0)
                .Max();

            return $"DH{(maxId + 1):D3}";
        }
    }
}
