using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using minishop.Dtos;
using minishop.Models;
using System.Data;
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

        [Authorize(Roles = "admin, user")]
        public IActionResult Index()
        {

            var user = _context.Users.Include(a => a.Cart!.CartItems).ThenInclude(c => c.Product).FirstOrDefault(a => a.Email == HttpContext.User.Identity!.Name);

            if (user == null)
                return BadRequest("Auth error (no user)");

            if (user.Cart == null)
                return BadRequest("user is not cart");

            var products = user.Cart!.CartItems;
            return View(products);
        }
        

        [HttpPost]
        [Authorize(Roles = "admin, user")]
        public IActionResult GetCount()
        {
            var user = _context.Users.Include(a => a.Cart!.CartItems).ThenInclude(c => c.Product).FirstOrDefault(a => a.Email == HttpContext.User.Identity!.Name);

            if (user == null)
                return BadRequest("error user login");

            if (user.Cart == null)
                return BadRequest("user is not cart");


            return Json(new { count = user.Cart!.CartItems.Count});
        }

        [HttpPost]
        [Authorize(Roles = "admin, user")]
        public IActionResult AddToCart(AddCard addCard)
        {
            if (!ModelState.IsValid)
                return BadRequest("error");
            var user = _context.Users.Include(a => a.Cart).FirstOrDefault(a => a.Email == HttpContext.User.Identity!.Name);

            if (user == null)
                return BadRequest();

            var product = _context.Products.Find(addCard.IdProduct)!;
            if (product == null)
                return BadRequest();
            var item = new CartItem() { Cart = user!.Cart!, Product = product, Count = addCard.Count };
            _context.CartItems.Add(item);
            _context.SaveChanges();
            var count = _context.CartItems.Where(a => a.CartId == user.Cart!.Id).Count();
            return Json(new { count = count });
        }
        [HttpDelete]
        [Authorize(Roles = "admin, user")]
        public IActionResult DeleteInCart(int id)
        {
            var user = _context.Users.Include(a => a.Cart!.CartItems).FirstOrDefault(a => a.Email == HttpContext.User.Identity!.Name);

            if (user == null)
                return BadRequest();

            if (id == 0)
                return BadRequest();
            if(user.Cart!.CartItems.FirstOrDefault(a =>a.Id == id)==null)
                return BadRequest();
            _context.CartItems.Remove(user.Cart.CartItems.FirstOrDefault(a => a.Id == id)!);
            _context.SaveChanges();
            return Json(new { count = user.Cart.CartItems.Count});
        }
    }
}