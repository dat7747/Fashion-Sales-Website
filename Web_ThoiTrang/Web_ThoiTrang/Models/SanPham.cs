namespace Web_ThoiTrang.Models
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
        public ICollection<HinhAnhSanPham> HinhAnhSanPham { get; set; }
    }

}
