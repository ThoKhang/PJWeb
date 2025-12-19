using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBNC.Models
{
    public class YeuCauDoiTra
    {
        [Key]
        public int idYeuCau { get; set; }

        [Column(TypeName = "char(5)")]
        public string idDonDat { get; set; }

        [ForeignKey("idDonDat")]
        public DonDatHang DonDatHang { get; set; }

        public string userId { get; set; }

        [ForeignKey("userId")]
        public ApplicationUser User { get; set; }

        [StringLength(300)]
        public string lyDo { get; set; }

        [StringLength(20)]
        public string trangThai { get; set; }

        public DateTime ngayTao { get; set; } = DateTime.Now;
    }
}
