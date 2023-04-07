using Microsoft.AspNetCore.Mvc;
using minishop.Dtos;
using minishop.Models;
using System.Diagnostics;

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

            foreach (var pr in products)
            {
                resProds.Add(new ProductCard()
                {
                    Id = pr.Id,
                    Price = pr.Price,
                    Title = pr.Name
                });
            }
            return View(resProds);
        }
    }
}