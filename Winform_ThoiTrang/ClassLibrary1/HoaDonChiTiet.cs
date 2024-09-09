using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class HoaDonChiTiet
    {
        public int HoaDonChiTietID { get; set; }
        public int HoaDonID { get; set; }
        public int SanPhamID { get; set; }
        public int SoLuong { get; set; }

        public string Size { get; set; }
        public decimal DonGia { get; set; }

        // Relationships
        public HoaDon HoaDon { get; set; }
        public SanPham SanPham { get; set; }
    }

}
