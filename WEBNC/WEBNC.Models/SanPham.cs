using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [ForeignKey("idCongTy")]
        [ValidateNever]
        public CongTy congTy { get; set; }
        public string idLoaiSanPham { get; set; }
        [ForeignKey("idLoaiSanPham")]
        [ValidateNever]
        public LoaiSanPham LoaiSanPham { get; set; }
        public decimal gia { get; set; }
        public string moTa { get; set; }
        public string imageLienQuan { get; set; }

        // Phải bỏ 3 dòng này phần code Đơn đặt hàng của tau mới chạy được do lỗi với hàm getAll (Dương)
        //public string thongSoSanPham { get; set; }
        //public int soLuongCanDuoi { get; set; }
        //public int soLuongHienCon { get; set; }
    }
}
