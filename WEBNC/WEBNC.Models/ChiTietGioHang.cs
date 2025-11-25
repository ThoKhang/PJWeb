using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WEBNC.Models
{
    public class ChiTietGioHang
    {
        [Key]
        public string idChiTietGioHang { get; set; } 

        public string idNguoiDung { get; set; } 

        //[ForeignKey("idNguoiDung")]
        //[ValidateNever]
        //public ApplicationUser user { get; set; } 

        public string idSanPham { get; set; } 
        [ValidateNever]
        public virtual SanPham SanPham { get; set; }

        public int soLuongTrongGio { get; set; }
    }
}