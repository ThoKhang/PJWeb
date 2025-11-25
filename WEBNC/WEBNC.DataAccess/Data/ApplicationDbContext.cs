using Microsoft.EntityFrameworkCore;
using WEBNC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WEBNC.Data
{
    // Đảm bảo bạn đã khai báo ApplicationUser trong Models
    public class ApplicationDbContext : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<SanPham> SanPham { get; set; }
        public DbSet<LoaiSanPham> LoaiSanPham { get; set; }
        public DbSet<CongTy> CongTy { get; set; }
        public DbSet<DonDatHang> DonDatHang { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHang { get; set; }
        //public DbSet<ChiTietGioHang> ChiTietGioHang { get; set; } 
        public DbSet<XaPhuong> XaPhuong { get; set; }
        public DbSet<Huyen> Huyen { get; set; }
        public DbSet<Tinh> Tinh { get; set; }
        //public DbSet<ApplicationUser> applicationUser { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Khóa chính kép cho ChiTietDonHang (Đã có)
            modelBuilder.Entity<ChiTietDonHang>()
                .HasKey(c => new { c.idDonDat, c.idSanPham });

            // 2. Khóa chính kép cho ChiTietGioHang (Theo mô hình ERD)
            modelBuilder.Entity<ChiTietGioHang>()
                .HasKey(c => new { c.idChiTietGioHang, c.idSanPham });

            // 3. Khắc phục cảnh báo decimal cho SanPham.gia
            modelBuilder.Entity<SanPham>()
                .Property(s => s.gia)
                .HasPrecision(18, 2);
        }
    }
}