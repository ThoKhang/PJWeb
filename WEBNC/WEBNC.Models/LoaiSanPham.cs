using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBNC.Models
{
    public class LoaiSanPham
    {
        [Key]
        public string idLoaiSanPham { get; set; }

        [Required]
        public string tenLoaiSanPham { get; set; }
    }
}
