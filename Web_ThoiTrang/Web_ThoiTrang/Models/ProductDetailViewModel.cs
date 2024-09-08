namespace Web_ThoiTrang.Models
{
    public class ProductDetailViewModel
    {
        public int SanPhamID { get; set; }
        public string TenSanPham { get; set; }
        public decimal Gia { get; set; }
        public string MoTa { get; set; }
        public string ChatLieu { get; set; }
        public string NhaSanXuat { get; set; }
        public List<HinhAnhSanPhamViewModel> HinhAnhSanPham { get; set; }
        public int CartItemCount { get; set; } 
    }

}
