using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class SanPham
    {
        public int SanPhamID { get; set; }
        public string TenSanPham { get; set; }
        public decimal Gia { get; set; }
        public string MoTa { get; set; }
        public int LoaiSanPhamID { get; set; }
        public string ChatLieu { get; set; }
        public string NhaSanXuat { get; set; }


        public ICollection<HinhAnhSanPham> HinhAnhSanPhams { get; set; }
    }
}
