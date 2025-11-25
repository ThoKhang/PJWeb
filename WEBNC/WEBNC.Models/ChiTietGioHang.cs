using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBNC.Models
{
    public class ChiTietGioHang
    {
        public int idChiTietGioHang { get; set; }

        public int idNguoiDung { get; set; }

        public int idSanPham { get; set; }

        public int soLuongTrongGio { get; set; }

        // Navigation properties (nếu bạn cần)
        //public virtual NguoiDung NguoiDung { get; set; }
        public virtual SanPham SanPham { get; set; }
    }
}

