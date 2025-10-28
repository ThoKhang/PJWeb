using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBNC.Models
{
    public class CongTy
    {
        [Key]
        public string idCongTy { get; set; }
        [Required]
        public string tenCongTy { get; set; }
        [ValidateNever]
        public ICollection<SanPham> SanPhams { get; set; }
    }
}
