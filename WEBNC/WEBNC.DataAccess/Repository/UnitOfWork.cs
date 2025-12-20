﻿using WEBNC.DataAccess.Repository.IRepository;
using WEBNC.Models;

namespace WEBNC.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            SanPham = new SanPhamRepository(_db);
            DonDatHang = new DonDatHangRepository(_db);
            ChiTietDonHang = new ChiTietDonHangRepository(_db);
            chiTietGioHang = new ChitietgiohangRepository(_db);
            LoaiSanPham = new LoaiSanPhamRepository(_db);
            ChatSession = new ChatSessionRepository(_db);
            ChatMessage = new ChatMessageRepository(_db);
            XaPhuong = new XaPhuongRepository(_db);

            DanhGia = new DanhGiaRepository(_db);
            HinhAnhDanhGia = new HinhAnhDanhGiaRepository(_db);
            ThanhToan = new ThanhToanRepository(_db);
            XaPhuong = new XaPhuongRepository(_db);


            DanhGia = new DanhGiaRepository(_db);
            HinhAnhDanhGia = new HinhAnhDanhGiaRepository(_db);
        }
        public ISanPhamRepository SanPham { get; private set; }
        public IDonDatHangRepository DonDatHang { get; private set; }
        public IChiTietDonHangRepository ChiTietDonHang { get; private set; }

        public IChiTietGioHangRepository chiTietGioHang { get; private set; }

        public ILoaiSanPhamRepository LoaiSanPham { get; private set; }

        public IChatSessionRepository ChatSession { get; private set; }
        public IChatMessageRepository ChatMessage { get; private set; }
        public IThanhToanRepository ThanhToan { get; private set; }
        public IXaPhuongRepository XaPhuong { get; private set; }

        public IDanhGiaRepository DanhGia { get; private set; }

        public IHinhAnhDanhGiaRepository HinhAnhDanhGia {get; private set; }
        public void save()
        {
            _db.SaveChanges();
        }
    }
}
