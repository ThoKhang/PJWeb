using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBNC.Models
{
    [Table("ChiTietDonHang")]
    public class ChiTietDonHang
    {
        [Key, Column(Order = 0, TypeName = "char(5)")]
        public string idDonDat { get; set; }

        [Key, Column(Order = 1, TypeName = "char(5)")]
        public string idSanPham { get; set; }

        [Required]
        public int soluong { get; set; }

        [Column(TypeName = "money")]
        public decimal donGia { get; set; }

        [ForeignKey("idDonDat")]
        public DonDatHang DonDatHang { get; set; }

        [ForeignKey("idSanPham")]
        public SanPham SanPham { get; set; }
    }
}
