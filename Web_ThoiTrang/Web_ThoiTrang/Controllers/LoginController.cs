using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Web_ThoiTrang.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace Web_ThoiTrang.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;
        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Thực hiện đăng nhập bằng Google
        [HttpPost]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        // Xử lý phản hồi từ Google
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (result?.Principal != null)
            {
                // Lấy thông tin người dùng từ Google
                var email = result.Principal.FindFirst(claim => claim.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
                var name = result.Principal.FindFirst(claim => claim.Type == System.Security.Claims.ClaimTypes.Name)?.Value;

                // Kiểm tra nếu khách hàng đã tồn tại
                var existingCustomer = await _context.KhachHang.FirstOrDefaultAsync(kh => kh.Email == email);
                if (existingCustomer == null)
                {
                    // Tạo khách hàng mới
                    var khachHang = new KhachHang
                    {
                        HoTen = name,
                        Email = email,
                        NgayTao = DateTime.Now
                    };
                    _context.KhachHang.Add(khachHang);
                    await _context.SaveChangesAsync();
                }

                // Chuyển hướng tới trang chính sau khi đăng nhập thành công
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index");
        }
    }
}
