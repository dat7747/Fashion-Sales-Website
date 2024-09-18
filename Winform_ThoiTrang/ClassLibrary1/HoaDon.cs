using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class HoaDon
    {
        public int HoaDonID { get; set; }
        public int KhachHangID { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public decimal TongTien { get; set; }
        public string PhuongThucThanhToan { get; set; }
        public KhachHang KhachHang { get; set; }

        public ICollection<HoaDonChiTiet> HoaDonChiTiet { get; set; }
    }

}
