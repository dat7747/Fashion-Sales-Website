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
            if (customerId == null)
            {
                // Xử lý nếu ID khách hàng không có trong session (người dùng chưa đăng nhập)
                return RedirectToAction("Index", "Login"); // Hoặc bất kỳ hành động nào bạn muốn thực hiện
            }

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

    }
}
