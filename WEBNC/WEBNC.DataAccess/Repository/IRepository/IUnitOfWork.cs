namespace WEBNC.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ISanPhamRepository SanPham { get; }
        IDonDatHangRepository DonDatHang { get; }
        IChiTietDonHangRepository ChiTietDonHang { get; }
        IChiTietGioHangRepository LoaiSanPham { get; }
        void save();
    }
}
