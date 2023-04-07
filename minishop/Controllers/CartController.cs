using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using minishop.Dtos;
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

        [HttpPost]
        public IActionResult AddToCard(AddCard addCard)
        {
            if (!ModelState.IsValid)
                return BadRequest("error");
            var user = _context.Users.Include(a =>a.Cart).FirstOrDefault(a=>a.Id ==1);
            var item = new CartItem(){ Cart = user!.Cart!, Product = _context.Products.Find(addCard.IdProduct)!, Count = addCard.Count};
            _context.CartItems.Add(item);
            _context.SaveChanges();
            return View();
        }
    }
}