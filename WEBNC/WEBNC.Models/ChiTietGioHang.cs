using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBNC.Models;

public class ChiTietGioHang
{
    [Key]
    public string idChiTietGioHang { get; set; }

    public string idNguoiDung { get; set; }
    [ForeignKey("idNguoiDung")]
    public ApplicationUser user { get; set; }

    public string idSanPham { get; set; }
    [ForeignKey("idSanPham")]
    public SanPham SanPham { get; set; }

    public int soLuongTrongGio { get; set; }
}
