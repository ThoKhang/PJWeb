using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBNC.Models
{
    public class ChiTietGioHang
    {
        [Key]
        [ValidateNever]
        public string idChiTietGioHang { get; set; }
        [ValidateNever]
        public string idNguoiDung { get; set; }
        [ForeignKey("idNguoiDung")]
        [ValidateNever]
        public ApplicationUser user { get; set; }

        public string idSanPham { get; set; }
        [ForeignKey("idSanPham")]
        [ValidateNever]
        public SanPham SanPham { get; set; }
        [Range(1,10001,ErrorMessage ="Vui lòng nhập từ 1 - 10000")]
        public int soLuongTrongGio { get; set; }
    }
}