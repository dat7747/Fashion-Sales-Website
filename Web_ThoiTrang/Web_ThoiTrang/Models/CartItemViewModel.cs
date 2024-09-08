namespace Web_ThoiTrang.Models
{
    public class CartItemViewModel
    {
        public int SanPhamID { get; set; }
        public string SanPhamTen { get; set; }
        public decimal SanPhamGia { get; set; }
        public int SoLuong { get; set; }
        public string Size { get; set; }
        public byte[] HinhAnhDaiDien { get; set; }
    }

}
