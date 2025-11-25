using Microsoft.EntityFrameworkCore;
using WEBNC.Models;

namespace WEBNC.Data
{
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
        //public DbSet<NguoiDung> NguoiDung { get; set; }
        public DbSet<XaPhuong> XaPhuong { get; set; }
        public DbSet<Huyen> Huyen { get; set; }
        public DbSet<Tinh> Tinh { get; set; }

        // Định nghĩa khóa chính kép cho ChiTietDonHang
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ChiTietDonHang>()
                .HasKey(c => new { c.idDonDat, c.idSanPham });
        }
    }
}
