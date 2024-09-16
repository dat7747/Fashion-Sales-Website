using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class PhieuNhap
    {
        public int PhieuNhapID { get; set; }  // Khóa chính
        public int NhaCungCapID { get; set; }  // Khóa ngoại tham chiếu đến bảng NhaCungCap
        public DateTime NgayNhap { get; set; } // Ngày nhập hàng
        public decimal TongTien { get; set; }  // Tổng tiền của phiếu nhập
        public string GhiChu { get; set; }     // Ghi chú 

        public NhaCungCap NhaCungCap { get; set; }

        public ICollection<ChiTietPhieuNhap> ChiTietPhieuNhap { get; set; }
    }
}
