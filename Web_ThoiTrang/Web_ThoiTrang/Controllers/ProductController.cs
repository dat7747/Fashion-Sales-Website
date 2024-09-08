using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_ThoiTrang.Models;

namespace Web_ThoiTrang.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var sanPhams = _context.SanPham
                .Select(sp => new
                {
                    sp.SanPhamID,
                    sp.TenSanPham,
                    sp.Gia,
                    HinhAnhDaiDien  = _context.HinhAnhSanPham
                    .Where(ha => ha.SanPhamID == sp.SanPhamID)
                    .Select(ha => ha.HinhAnh)
                    .FirstOrDefault()
                }).ToList();
            return View(sanPhams);
        }

        public IActionResult DetailProduct(int id)
        {
            // Lấy ID khách hàng từ session
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            var product = _context.SanPham
                .Include(p => p.HinhAnhSanPham)
                .FirstOrDefault(p => p.SanPhamID == id);

            if (product == null)
            {
                return NotFound();
            }

            // Tính toán số sản phẩm trong giỏ hàng
            var cartItemsCount = _context.CartItem
                .Where(c => c.KhachHangID == customerId)
                .Sum(c => c.SoLuong); // Đếm tổng số lượng sản phẩm trong giỏ hàng

            // Thêm số lượng sản phẩm vào ViewBag để truyền đến View
            ViewBag.CartItemCount = cartItemsCount;

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> GetCartItemCount()
        {
            var userId = User.Identity.Name;
            var customer = await _context.KhachHang.FirstOrDefaultAsync(kh => kh.HoTen == userId);

            if (customer == null)
            {
                return Json(new { count = 0 });
            }

            var itemCount = await _context.CartItem
                .Where(ci => ci.KhachHangID == customer.KhachHangID)
                .SumAsync(ci => ci.SoLuong);

            return Json(new { count = itemCount });
        }

        public IActionResult Search(string searchTerm)
        {
            var sanPhams = _context.SanPham
                .Where(sp => string.IsNullOrEmpty(searchTerm) || sp.TenSanPham.Contains(searchTerm)) // Kiểm tra nếu searchTerm rỗng
                .Select(sp => new
                {
                    sp.SanPhamID,
                    sp.TenSanPham,
                    Gia = sp.Gia.ToString("N0"), // Định dạng giá thành chuỗi có dấu phân cách
                    HinhAnhDaiDien = _context.HinhAnhSanPham
                        .Where(ha => ha.SanPhamID == sp.SanPhamID)
                        .Select(ha => ha.HinhAnh)
                        .FirstOrDefault() // Giữ nguyên giá trị byte[]
                })
                .ToList()
                .Select(sp => new
                {
                    sp.SanPhamID,
                    sp.TenSanPham,
                    sp.Gia,
                    HinhAnhDaiDien = sp.HinhAnhDaiDien != null
                        ? Convert.ToBase64String(sp.HinhAnhDaiDien)
                        : string.Empty // Nếu hình ảnh không có, dùng chuỗi rỗng
                });

            return Json(sanPhams);
        }
    }
}
