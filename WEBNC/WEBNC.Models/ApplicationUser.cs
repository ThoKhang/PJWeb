//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace WEBNC.Models
//{
//    public class ApplicationUser : IdentityUser
//    {
//        [Required]
//        public string hoTen { get; set; }
//        public string? soNha { get; set; }
//        public string idPhuongXa { get; set; }
//        [ForeignKey("idPhuongXa")]
//        [ValidateNever]
//        public XaPhuong xaPhuong { get; set; }
//    }
//}
