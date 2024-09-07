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
            var product = _context.SanPham
                .Include(p => p.HinhAnhSanPham)
                .FirstOrDefault(p => p.SanPhamID == id);

            if (product == null) { 
                return NotFound();
            }

            return View(product);
        }
	}
}
