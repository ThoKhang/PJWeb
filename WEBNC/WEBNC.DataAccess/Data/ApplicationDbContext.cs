using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WEBNC.Models;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
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
    public DbSet<ChiTietGioHang> ChiTietGioHang { get; set; }
    public DbSet<XaPhuong> XaPhuong { get; set; }
    //public DbSet<Huyen> Huyen { get; set; }
    public DbSet<Tinh> Tinh { get; set; }
    public DbSet<ChatSession> ChatSessions { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<ThanhToan> ThanhToan { get; set; }
    public DbSet<DanhGiaSanPham> DanhGiaSanPham { get; set; }
    public DbSet<ThongBao> ThongBao { get; set; }
    public DbSet<YeuCauDoiTra> YeuCauDoiTra { get; set; }
    public DbSet<HinhAnhDanhGia> HinhAnhDanhGia { get; set; }


    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    base.OnModelCreating(modelBuilder);

    //    // Khóa chính kép ChiTietDonHang
    //    modelBuilder.Entity<ChiTietDonHang>()
    //        .HasKey(c => new { c.idDonDat, c.idSanPham });

    //    // Decimal chính xác cho SanPham.gia
    //    modelBuilder.Entity<SanPham>()
    //        .Property(s => s.gia)
    //        .HasPrecision(18, 2);

    //    // Quan hệ ApplicationUser - XaPhuong
    //    modelBuilder.Entity<ApplicationUser>()
    //        .HasOne(u => u.xaPhuong)
    //        .WithMany()
    //        .HasForeignKey(u => u.idPhuongXa)
    //        .OnDelete(DeleteBehavior.Restrict);
    //}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ChiTietDonHang>()
            .HasKey(c => new { c.idDonDat, c.idSanPham });

        modelBuilder.Entity<SanPham>()
            .Property(s => s.gia)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ThanhToan>()
            .Property(t => t.soTien)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ApplicationUser>()
            .HasOne(u => u.xaPhuong)
            .WithMany()
            .HasForeignKey(u => u.idPhuongXa)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<DonDatHang>()
            .HasOne(d => d.nguoiDung)        
            .WithMany(u => u.DonDatHangs)    
            .HasForeignKey(d => d.idNguoiDung)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<ChiTietDonHang>()
         .HasOne(c => c.DonDatHang)
         .WithMany(d => d.ChiTietDonHang)
         .HasForeignKey(c => c.idDonDat);


        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

}
