using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBNC.Models
{
    public class ThongBao
    {
        [Key]
        public int idThongBao { get; set; }

        [Required]
        public string userId { get; set; }

        [ForeignKey("userId")]
        public ApplicationUser User { get; set; }

        [StringLength(200)]
        public string tieuDe { get; set; }

        [StringLength(500)]
        public string noiDung { get; set; }

        public bool daDoc { get; set; } = false;

        public DateTime ngayTao { get; set; } = DateTime.Now;
    }
}
