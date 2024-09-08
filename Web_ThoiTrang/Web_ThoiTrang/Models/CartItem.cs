namespace Web_ThoiTrang.Models
{
	public class CartItem
	{
		public int CartItemID { get; set; }
		public int SanPhamID { get; set; }
		public SanPham SanPham { get; set; } // Include navigation property
		public int KhachHangID { get; set; }
		public KhachHang KhachHang { get; set; } // Include navigation property
		public int SoLuong { get; set; }
		public string Size { get; set; }
	}
}
