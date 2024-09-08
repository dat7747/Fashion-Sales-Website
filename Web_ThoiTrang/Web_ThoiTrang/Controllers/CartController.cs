using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_ThoiTrang.Models;

namespace Web_ThoiTrang.Controllers
{
    public class CartController : Controller
    {
		private readonly ApplicationDbContext _context;

		public CartController(ApplicationDbContext context)
		{
			_context = context;
		}
        public async Task<IActionResult> Index()
        {
            var userId = User.Identity.Name; // Lấy tên người dùng hiện tại
            var customer = await _context.KhachHang.FirstOrDefaultAsync(kh => kh.HoTen == userId);

            if (customer == null)
            {
                return Unauthorized(); // Trả về nếu người dùng chưa đăng nhập
            }

            // Truy vấn các mục giỏ hàng dựa trên KhachHangID từ bảng CartItem
            var cartItems = await _context.CartItem
                .Where(ci => ci.KhachHangID == customer.KhachHangID) // Lọc dựa trên KhachHangID
                .Include(ci => ci.SanPham) // Bao gồm thông tin sản phẩm
                .ThenInclude(sp => sp.HinhAnhSanPham) // Bao gồm hình ảnh sản phẩm
                .Select(ci => new CartItemViewModel
                {
                    SanPhamID = ci.SanPhamID,
                    SanPhamTen = ci.SanPham.TenSanPham,
                    SanPhamGia = ci.SanPham.Gia,
                    SoLuong = ci.SoLuong,
                    Size = ci.Size,
                    HinhAnhDaiDien = ci.SanPham.HinhAnhSanPham
                        .Select(ha => ha.HinhAnh) // Lấy hình ảnh đầu tiên hoặc theo logic của bạn
                        .FirstOrDefault()
                })
                .ToListAsync();

            return View(cartItems);
        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(int sanPhamId, int soLuong, string size)
        {
            if (sanPhamId <= 0 || soLuong <= 0 || string.IsNullOrEmpty(size))
            {
                return BadRequest("Invalid input"); // Hoặc điều chỉnh theo yêu cầu của bạn
            }

            var userId = User.Identity.Name; // Hoặc lấy ID người dùng từ Claims
            var customer = await _context.KhachHang.FirstOrDefaultAsync(kh => kh.HoTen == userId);

            if (customer == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var cartItem = new CartItem
            {
                SanPhamID = sanPhamId,
                KhachHangID = customer.KhachHangID,
                SoLuong = soLuong,
                Size = size
            };

            _context.CartItem.Add(cartItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int sanPhamId, int newQuantity)
        {
            if (sanPhamId <= 0 || newQuantity <= 0)
            {
                return BadRequest("Invalid input");
            }

            var userId = User.Identity.Name;
            var customer = await _context.KhachHang.FirstOrDefaultAsync(kh => kh.HoTen == userId);

            if (customer == null)
            {
                return Unauthorized();
            }

            var cartItem = await _context.CartItem
                .FirstOrDefaultAsync(ci => ci.SanPhamID == sanPhamId && ci.KhachHangID == customer.KhachHangID);

            if (cartItem == null)
            {
                return NotFound();
            }

            cartItem.SoLuong = newQuantity;
            _context.CartItem.Update(cartItem);
            await _context.SaveChangesAsync();

            // Tính toán giá tổng mới
            var total = await _context.CartItem
                .Where(ci => ci.KhachHangID == customer.KhachHangID)
                .SumAsync(ci => ci.SanPham.Gia * ci.SoLuong);

            return Json(new { total = total });
        }
        [HttpPost]
        public async Task<IActionResult> RemoveItem(int sanPhamId)
        {
            if (sanPhamId <= 0)
            {
                return BadRequest("Invalid product ID");
            }

            // Lấy thông tin người dùng hiện tại
            var userId = User.Identity.Name;
            var customer = await _context.KhachHang.FirstOrDefaultAsync(kh => kh.HoTen == userId);

            if (customer == null)
            {
                return Unauthorized(); // Người dùng chưa đăng nhập
            }

            // Tìm sản phẩm trong giỏ hàng của khách hàng
            var cartItem = await _context.CartItem
                .FirstOrDefaultAsync(ci => ci.SanPhamID == sanPhamId && ci.KhachHangID == customer.KhachHangID);

            if (cartItem == null)
            {
                return NotFound(); // Không tìm thấy sản phẩm trong giỏ hàng
            }

            // Xóa sản phẩm khỏi giỏ hàng
            _context.CartItem.Remove(cartItem);
            await _context.SaveChangesAsync();

            // Tính toán tổng giá mới của giỏ hàng
            var total = await _context.CartItem
                .Where(ci => ci.KhachHangID == customer.KhachHangID)
                .SumAsync(ci => ci.SanPham.Gia * ci.SoLuong);

            // Trả về tổng giá mới
            return Json(new { total = total });
        }

    }
}
