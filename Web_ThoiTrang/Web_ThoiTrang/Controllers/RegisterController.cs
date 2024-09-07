using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_ThoiTrang.Models;
using BCrypt.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Web_ThoiTrang.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegisterController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(KhachHang model, string password)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Kiểm tra nếu email đã tồn tại
            var existingCustomer = await _context.KhachHang
                .FirstOrDefaultAsync(kh => kh.Email == model.Email);

            if (existingCustomer != null)
            {
                ModelState.AddModelError("Email", "Email đã được đăng ký");
                return View(model);
            }

            // Mã hóa mật khẩu trước khi lưu
            if (!string.IsNullOrEmpty(password))
            {
                model.Password = BCrypt.Net.BCrypt.HashPassword(password);
            }
            else
            {
                // Tạo một giá trị đại diện cho mật khẩu nếu đăng ký qua Google
                model.Password = BCrypt.Net.BCrypt.HashPassword("google_auth_placeholder");
            }

            // Lưu thông tin khách hàng vào CSDL
            _context.KhachHang.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult ExternalLogin(string provider)
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("ExternalLoginCallback")
            };
            return Challenge(properties, provider);
        }

        public async Task<IActionResult> ExternalLoginCallback()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (result?.Principal != null)
            {
                var email = result.Principal.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
                var name = result.Principal.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.Name)?.Value;

                // Kiểm tra nếu email là null
                if (string.IsNullOrEmpty(email))
                {
                    return RedirectToAction("Index", "Register");
                }

                var existingCustomer = await _context.KhachHang
                    .FirstOrDefaultAsync(kh => kh.Email == email);

                if (existingCustomer == null)
                {
                    var khachHang = new KhachHang
                    {
                        HoTen = name,
                        Email = email,
                        NgayTao = DateTime.Now,
                        Password = BCrypt.Net.BCrypt.HashPassword("google_auth_placeholder") // Mã hóa giá trị đại diện
                    };
                    _context.KhachHang.Add(khachHang);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Register");
        }
    }
}
