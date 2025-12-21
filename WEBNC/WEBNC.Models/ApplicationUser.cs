using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBNC.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? hoTen { get; set; }

        public string? soNha { get; set; }

        [Column(TypeName = "char(5)")]
        public string? idPhuongXa { get; set; }

        [ForeignKey("idPhuongXa")]
        [ValidateNever]
        public XaPhuong xaPhuong { get; set; }
        public ICollection<DonDatHang> DonDatHangs { get; set; }
        public bool IsOtpVerified { get; set; } = false;
    }
}
