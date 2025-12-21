using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBNC.Models
{
    public class ThanhToan
    {
        [Key]
        public int idThanhToan { get; set; }

         [Column(TypeName = "char(5)")]
        [ForeignKey("idDonDat")]
        public DonDatHang DonDatHang { get; set; }

        public string phuongThuc { get; set; }

        public decimal soTien { get; set; }

        public bool daThanhToan { get; set; }

        public DateTime? ngayThanhToan { get; set; }

        public string maGiaoDich { get; set; }
    }
}
