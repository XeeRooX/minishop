using Microsoft.AspNetCore.Mvc;

namespace minishop.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Details()
        {
            return View();
        }

        public IActionResult Create() 
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }
    }
}
