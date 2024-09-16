using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ChiTietPhieuNhap
    {
        public int ChiTietPhieuNhapID { get; set; }  // Khóa chính
        public int PhieuNhapID { get; set; }         // Khóa ngoại tham chiếu đến bảng PhieuNhap
        public int SanPhamID { get; set; }           // Khóa ngoại tham chiếu đến bảng SanPham
        public int SoLuong { get; set; }             // Số lượng sản phẩm
        public string KichThuoc { get; set; }        // Kích thước sản phẩm
        public decimal GiaNhap { get; set; }         // Giá nhập của sản phẩm

        public PhieuNhap PhieuNhap { get; set; }
        public SanPham SanPham { get; set; }
    }

}
