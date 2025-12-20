using System.ComponentModel.DataAnnotations.Schema;
using WEBNC.Models;

[Table("ChiTietDonHang")]
public class ChiTietDonHang
{
    [Column(TypeName = "char(5)")]
    public string idDonDat { get; set; }

    [Column(TypeName = "char(5)")]
    public string idSanPham { get; set; }

    public int soluong { get; set; }

    [Column(TypeName = "money")]
    public decimal donGia { get; set; }

    [ForeignKey("idDonDat")]
    public DonDatHang DonDatHang { get; set; }

    [ForeignKey("idSanPham")]
    public SanPham SanPham { get; set; }
}
