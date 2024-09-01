using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CartItem
    {
        public int CartItemID { get; set; }  // ID của mục giỏ hàng
        public int SanPhamID { get; set; }   // ID của sản phẩm
        public int? KhachHangID { get; set; }   // ID của khách hàng (nullable nếu chưa đăng nhập)
        public int SoLuong { get; set; }    // Số lượng sản phẩm
        public string Size { get; set; }    // Size sản phẩm

        // Liên kết với đối tượng SanPham và KhachHang
        public virtual SanPham SanPham { get; set; }
        public virtual KhachHang KhachHang { get; set; }
    }


}
