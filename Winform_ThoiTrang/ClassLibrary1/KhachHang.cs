using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class KhachHang
    {
        public int KhachHangID { get; set; }  // Khóa chính
        public string HoTen { get; set; }  // Họ tên khách hàng
        public string Email { get; set; }  // Email khách hàng
        public string SoDienThoai { get; set; }  // Số điện thoại
        public string DiaChi { get; set; }  // Địa chỉ khách hàng
        public DateTime NgayTao { get; set; }  // Ngày tạo tài khoản
    }
}
