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
        IThanhToanRepository ThanhToan { get; }
        IXaPhuongRepository XaPhuong { get; }
        IDanhGiaRepository DanhGia { get; }
        IHinhAnhDanhGiaRepository HinhAnhDanhGia { get; }
        void Save();
    }
}
