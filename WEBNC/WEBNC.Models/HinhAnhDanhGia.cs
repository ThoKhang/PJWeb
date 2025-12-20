using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBNC.Models
{
    public class HinhAnhDanhGia
    {
        [Key]
        public int idHinhAnh { get; set; }

        public int? idDanhGia { get; set; }

        [ForeignKey("idDanhGia")]
        [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
        public DanhGiaSanPham DanhGiaSanPham { get; set; }

        [Required]
        public string imageUrl { get; set; }
    }
}
