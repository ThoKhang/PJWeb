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
    public class XaPhuong
    {
        [Key]
        [Column(TypeName = "char(5)")]
        public string idXaPhuong { get; set; }

        public string idHuyen { get; set; }

        [ForeignKey("idHuyen")]
        [ValidateNever]
        public Huyen Huyen { get; set; }

        public string tenXaPhuong { get; set; }
    }
}

