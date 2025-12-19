using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBNC.Models
{
    [Table("DonDatHang")]
    public class DonDatHang
    {
        [Key]
        [Column(TypeName = "char(5)")]
        public string idDonDat { get; set; }

        [Required]
        public string idNguoiDung { get; set; }

        //[ForeignKey("idNguoiDung")]
        //public NhanVien nhanVien { get; set; }

        [Required]
        [Column(TypeName = "char(11)")]
        public string sdtGiaoHang { get; set; }

        [Required]
        [StringLength(50)]
        public string soNha { get; set; }

        [Required]
        [StringLength(20)]
        public string trangThai { get; set; }

        [Required]
        [StringLength(15)]
        public string thanhToan { get; set; }

        public DateTime? ngayDat { get; set; }
        public DateTime? ngayThanhToan { get; set; }
        public DateTime? ngayGiaoDuKien { get; set; }
        public bool daThanhToan { get; set; } = false;

        public ICollection<ChiTietDonHang> ChiTietDonHang { get; set; }
    }
}
