using Microsoft.AspNetCore.Mvc;

namespace Web_ThoiTrang.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
