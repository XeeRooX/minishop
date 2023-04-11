using Microsoft.AspNetCore.Mvc;
using minishop.Dtos;
using minishop.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace minishop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.Take(8).ToList();
            var resProds = new List<ProductCard>();

            var userTemp = _context.Users.FirstOrDefault(u => u.Email == HttpContext.User.Identity!.Name);
            int userId = userTemp != null ? userTemp.Id : 0;

            foreach (var pr in products)
            {
                resProds.Add(new ProductCard()
                {
                    Id = pr.Id,
                    Price = pr.Price,
                    Title = pr.Name
                });
            }

            var user = _context.Users.Include(u => u.Cart!.CartItems).FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                foreach (var pr in resProds)
                {
                    if (user.Cart!.CartItems.FirstOrDefault(ci => ci.ProductId == pr.Id) != null)
                    {
                        pr.InCart = true;
                    }
                }
            }

            return View(resProds);
        }
    }
}