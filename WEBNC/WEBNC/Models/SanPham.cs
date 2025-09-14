using System.ComponentModel.DataAnnotations;

namespace WEBNC.Models
{
    public class SanPham
    {
        [Key]
        public string idSanPham { get; set; }
        [Required]
        public string tenSanPham { get; set; }
        public string imageURL { get; set; }
        public string idCongTy { get; set; }
        public string idLoaiSanPham { get; set; }
    }
}
