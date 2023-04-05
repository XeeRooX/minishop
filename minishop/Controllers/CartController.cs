using Microsoft.AspNetCore.Mvc;
using minishop.Models;
using System.Diagnostics;

namespace minishop.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}