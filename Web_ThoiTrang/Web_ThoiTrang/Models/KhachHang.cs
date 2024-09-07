using System.ComponentModel.DataAnnotations;

namespace Web_ThoiTrang.Models
{
    public class KhachHang
    {
        [Key]
        public int KhachHangID { get; set; }

        [MaxLength(100)]
        public string HoTen { get; set; }

        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(15)]
        public string SoDienThoai { get; set; }

        [MaxLength(255)]
        public string DiaChi { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.Now;
    }
}
