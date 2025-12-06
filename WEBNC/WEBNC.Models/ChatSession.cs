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
    public class ChatSession
    {
        [Key]
        public int idSession { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(450)")]
        public string userId { get; set; }

        [ForeignKey("userId")]
        [ValidateNever]
        public ApplicationUser User { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string title { get; set; } = "Cuộc trò chuyện mới";

        public DateTime createdAt { get; set; } = DateTime.Now;
    }
}
