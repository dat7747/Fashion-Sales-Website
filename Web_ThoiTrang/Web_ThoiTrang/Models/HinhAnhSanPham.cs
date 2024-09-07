using System.ComponentModel.DataAnnotations;

namespace Web_ThoiTrang.Models
{
    public class HinhAnhSanPham
    {
        [Key]
        public int HinhAnhID { get; set; }
        public int SanPhamID { get; set; }
        public byte[] HinhAnh { get; set; }

        public SanPham SanPham { get; set; }
    }
}
