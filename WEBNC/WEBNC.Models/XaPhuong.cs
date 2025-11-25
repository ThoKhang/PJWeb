using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBNC.Models
{
    public class XaPhuong
    {
        public string idXaPhuong { get; set; }
        public string idHuyen { get; set; }
        public string tenXaPhuong { get; set; }
        public virtual Huyen Huyen { get; set; }
    }
}

