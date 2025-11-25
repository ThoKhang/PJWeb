using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBNC.Models
{
    public class Tinh
    {
        [Key]
        public string idTinh { get; set; }
        public string tenTinh { get; set; }

    }
}
