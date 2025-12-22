using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBNC.DataAccess.Repository.IRepository;
using WEBNC.Models;

namespace WEBNC.DataAccess.Repository
{
    public class DanhGiaRepository : Repository<DanhGiaSanPham>, IDanhGiaRepository
    {
        public readonly ApplicationDbContext _db;
        public DanhGiaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public bool DaDanhGia(string idSanPham, string userId, string? idDonDat)
        {
            var danhGia = GetFirstOrDefault(u => u.idSanPham == idSanPham && u.userId == userId && u.idDonDat == idDonDat);
            return danhGia != null;
        }

        public IEnumerable<DanhGiaSanPham> GetDanhGiaBySanPham(string idSanPham)
        {
            var danhGiaList=GetAll(u=>u.idSanPham==idSanPham,includeProperties: "User,HinhAnhDanhGias");
            return danhGiaList;
        }

        public double TinhDiemTrungBinh(string idSanPham)
        {
            var danhGiaList=GetAll(u=>u.idSanPham==idSanPham);
            if (danhGiaList.Count() == 0)
            {
                return 0;
            }
            double tongDiem = danhGiaList.Sum(u => u.soSao);
            return tongDiem / danhGiaList.Count();
        }

        public void Update(DanhGiaSanPham obj)
        {
            var objFormdb =_db.DanhGiaSanPham.FirstOrDefault(u => u.idDanhGia == obj.idDanhGia);
            if (objFormdb != null)
            {
                objFormdb.noiDung= obj.noiDung;
                objFormdb.soSao= obj.soSao;
            }
        }
    }
}
