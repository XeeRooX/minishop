using Microsoft.AspNetCore.Mvc;

namespace minishop.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Details()
        {
            return View();
        }
    }
}
