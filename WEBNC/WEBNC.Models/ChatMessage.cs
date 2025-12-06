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
    public class ChatMessage
    {
        [Key]
        public int idMessage { get; set; }

        public int idSession { get; set; }

        [ForeignKey("idSession")]
        [ValidateNever]
        public ChatSession session { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string role { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string message { get; set; }

        public DateTime createdAt { get; set; } = DateTime.Now;
    }
}
