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
        IXaPhuongRepository XaPhuong { get; }
        IDanhGiaRepository DanhGia { get; }
        IHinhAnhDanhGiaRepository HinhAnhDanhGia { get; }
        IThanhToanRepository ThanhToan { get; }
        void save();
    }
}
