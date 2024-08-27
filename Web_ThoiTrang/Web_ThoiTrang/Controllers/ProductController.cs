using Microsoft.AspNetCore.Mvc;

namespace Web_ThoiTrang.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DetailProdcut()
        {
            return View();
        }
    }
}
