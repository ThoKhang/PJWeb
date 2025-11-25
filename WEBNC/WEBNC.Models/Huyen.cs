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
    public class Huyen
    {
        [Key]
        public string idHuyen { get; set; }

        public string idTinh { get; set; }

        [ForeignKey("idTinh")]
        [ValidateNever]
        public Tinh Tinh { get; set; }

        public string tenHuyen { get; set; }

        [ValidateNever]
        public ICollection<XaPhuong> XaPhuongs { get; set; }
    }
}

