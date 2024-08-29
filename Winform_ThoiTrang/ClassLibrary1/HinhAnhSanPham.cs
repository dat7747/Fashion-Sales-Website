using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public  class HinhAnhSanPham
    {
        public int HinhAnhID { get; set; }
        public int SanPhamID { get; set; }
        public byte[] HinhAnh { get; set; }

        public SanPham SanPham { get; set; }
    }
}
