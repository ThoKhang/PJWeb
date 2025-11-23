using Microsoft.EntityFrameworkCore;
using WEBNC.Data;
using WEBNC.DataAccess.Repository.IRepository;
using WEBNC.Models;

namespace WEBNC.DataAccess.Repository
{
    public class SanPhamRepository : Repository<SanPham>, ISanPhamRepository
    {
        public readonly ApplicationDbContext _db;
        public SanPhamRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(SanPham obj)
        {
            var objFromDb = _db.SanPham.SingleOrDefault(u => u.idSanPham == obj.idSanPham);
            if (objFromDb != null)
            {
                objFromDb.tenSanPham = obj.tenSanPham;
                objFromDb.idCongTy = obj.idCongTy;
                objFromDb.idLoaiSanPham = obj.idLoaiSanPham;
                if (!string.IsNullOrEmpty(obj.imageURL))
                {
                    objFromDb.imageURL = obj.imageURL;
                }
            }
        }
        public IEnumerable<SanPham> LayTop10SanPhamBanChay()
        {
            var query = _db.ChiTietDonHang
                .Include(ct => ct.SanPham)
                .Include(ct => ct.DonDatHang)
                .Where(ct =>
                    ct.DonDatHang.trangThai == "Đã giao" &&
                    ct.DonDatHang.thanhToan == "Đã thanh toán")
                .GroupBy(ct => new
                {
                    ct.SanPham.idSanPham,
                    ct.SanPham.tenSanPham,
                    ct.SanPham.imageURL,
                    ct.SanPham.gia
                })
                .Select(g => new
                {
                    SanPham = g.Key,
                    TongSoLuongBan = g.Sum(x => x.soluong)
                })
                .OrderByDescending(g => g.TongSoLuongBan)
                .Take(10)
                .AsEnumerable()
                .Select(g => new SanPham
                {
                    idSanPham = g.SanPham.idSanPham,
                    tenSanPham = g.SanPham.tenSanPham,
                    imageURL = g.SanPham.imageURL,
                    gia = g.SanPham.gia
                });

            return query.ToList();
        }
    }
}
