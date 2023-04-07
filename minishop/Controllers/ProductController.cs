using Microsoft.AspNetCore.Mvc;
using minishop.Dtos;

namespace minishop.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Details()
        {
            return View();
        }

        public IActionResult Index(string category)
        {
            //Commit
            ViewBag.Category = category;
            List<ProductCard> products = new List<ProductCard>() {  
                new ProductCard()
                {
                    Id = 1,
                    Price = 200,
                    Title = "Watch1"
                },
                new ProductCard()
                {
                    Id = 2,
                    Price = 300,
                    Title = "Watch2"
                },
                new ProductCard()
                {
                    Id = 3,
                    Price = 400,
                    Title = "Watch3"
                },
                new ProductCard()
                {
                    Id = 4,
                    Price = 500,
                    Title = "Watch3"
                }
            };

            return View(products);
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
