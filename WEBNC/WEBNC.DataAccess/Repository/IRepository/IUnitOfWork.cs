namespace WEBNC.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ISanPhamRepository SanPham { get; }
        IDonDatHangRepository DonDatHang { get; }
        IChiTietDonHangRepository ChiTietDonHang { get; }
        IChiTietGioHangRepository chiTietGioHang { get; }
        ILoaiSanPhamRepository LoaiSanPham { get; }
        IChatSessionRepository ChatSession { get; }
        IChatMessageRepository ChatMessage { get; }
        IDanhGiaRepository DanhGia { get; }
        IHinhAnhDanhGiaRepository HinhAnhDanhGia { get; }
        IXaPhuongRepository XaPhuong { get; }
        void save();
    }
}
