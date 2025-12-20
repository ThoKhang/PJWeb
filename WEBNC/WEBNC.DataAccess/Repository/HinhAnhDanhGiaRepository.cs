using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBNC.DataAccess.Repository.IRepository;
using WEBNC.Models;

namespace WEBNC.DataAccess.Repository
{
    public class HinhAnhDanhGiaRepository : Repository<HinhAnhDanhGia>, IHinhAnhDanhGiaRepository
    {
        public readonly ApplicationDbContext _db;
        public HinhAnhDanhGiaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<HinhAnhDanhGia> GetByDanhGia(int idDanhGia)
        {
            return GetAll(x => x.idDanhGia == idDanhGia);
        }

        public void RemoveByDanhGia(int idDanhGia)
        {
            var images = GetAll(x => x.idDanhGia == idDanhGia);
            RemoveRange(images);
        }
    }
}
