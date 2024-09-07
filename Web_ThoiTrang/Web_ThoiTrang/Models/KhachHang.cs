using System;
using System.ComponentModel.DataAnnotations;

namespace Web_ThoiTrang.Models
{
    public class KhachHang
    {
        [Key]
        public int KhachHangID { get; set; }

        [MaxLength(100)]
        public string? HoTen { get; set; } // Thay đổi thành string? để cho phép null

        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; } // Thay đổi thành string? để cho phép null

        [MaxLength(15)]
        public string? SoDienThoai { get; set; } // Thay đổi thành string? để cho phép null

        [MaxLength(255)]
        public string? DiaChi { get; set; } // Thay đổi thành string? để cho phép null

        public DateTime NgayTao { get; set; } = DateTime.Now;

        [MaxLength(255)]
        public string? Password { get; set; } // Thay đổi thành string? để cho phép null
    }
}
