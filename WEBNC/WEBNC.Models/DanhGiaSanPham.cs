using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBNC.Models
{
    public class DanhGiaSanPham
    {
        [Key]
        public int idDanhGia { get; set; }
        [Required]
        public string idSanPham { get; set; }
        [ForeignKey("idSanPham")]
        public SanPham SanPham { get; set; }
        [Required]
        public string userId { get; set; }
        [ForeignKey("userId")]
        public ApplicationUser User { get; set; }
        [Column(TypeName = "char(5)")]
        public string idDonDat { get; set; }
        [ForeignKey("idDonDat")]
        public DonDatHang DonDatHang { get; set; }
        [Range(1, 5)]
        public int soSao { get; set; }
        [StringLength(500)]
        public string noiDung { get; set; }
        public DateTime ngayDanhGia { get; set; } = DateTime.Now;
    }
}
