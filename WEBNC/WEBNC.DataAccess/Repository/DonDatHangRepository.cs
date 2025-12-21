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
            var existingIds = _db.DonDatHang
                .Select(d => d.idDonDat)
                .Where(id => id.StartsWith("DH"))
                .ToList();

            if (!existingIds.Any())
            {
                return "DH001";
            }

            long maxId = 0;
            foreach (var id in existingIds)
            {
                if (id.Length > 2 && long.TryParse(id.Substring(2), out long currentId))
                {
                    if (currentId > maxId)
                    {
                        maxId = currentId;
                    }
                }
            }

            return "DH" + (maxId + 1).ToString("D3");
        }
    }
}