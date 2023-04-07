using Microsoft.AspNetCore.Mvc;
using minishop.Dtos;
using minishop.Models;

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
            IEnumerable<Product> products;
            if (category == "mech")
            {
                
            }
            else if (category == "elec")
            {

            }
            else if (category == "smart")
            {

            }
            else
            {

            }

            ViewBag.Category = category;
            List<ProductCard> products1 = new List<ProductCard>() {
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

            return View(products1);
        }
    }
}
