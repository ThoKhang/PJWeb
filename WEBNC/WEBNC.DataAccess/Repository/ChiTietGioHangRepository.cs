using Microsoft.EntityFrameworkCore;
using WEBNC.DataAccess.Repository.IRepository;
using WEBNC.Models;

namespace WEBNC.DataAccess.Repository
{
    public class ChitietgiohangRepository : Repository<ChiTietGioHang>, IChiTietGioHangRepository
    {
        private readonly ApplicationDbContext _db;

        public ChitietgiohangRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public int DecrementCount(ChiTietGioHang chiTietGioHang, int count)
        {
            chiTietGioHang.soLuongTrongGio -= count;
            return chiTietGioHang.soLuongTrongGio;
        }

        public int IncrementCount(ChiTietGioHang chiTietGioHang, int count)
        {
            chiTietGioHang.soLuongTrongGio += count;
            return chiTietGioHang.soLuongTrongGio;
        }

        public void Update(ChiTietGioHang obj)
        {
            _db.ChiTietGioHang.Update(obj);
        }
    }
}
