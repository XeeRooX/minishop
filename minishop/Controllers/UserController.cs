using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using minishop.Dtos;
using minishop.Models;
using SQLitePCL;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace minishop.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context = null!;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("/Login")]
        public IActionResult Login()
        {
            return View();
        }

        [Route("/Login")]
        [HttpPost]
        public IActionResult Login(LoginModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("error send model");
            }
            var user = _context.Users.Include(a=>a.Role).FirstOrDefault(a => a.Email == userModel.Email);
            if (user == null)
            {
                ViewBag.Message = "Пользователь с таким email не найден";
                return View(userModel);
            }
            if (user.Password != GetHash(userModel.Password))
            {
                ViewBag.Message = "Неверный пароль";
                return View(userModel);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name)
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            HttpContext.SignInAsync(claimsPrincipal);

            return Redirect("/");
        }

        [Route("/Register")]
        public IActionResult Register()
        {
            return View();
        }


        [Route("/Register")]
        [HttpPost]
        public IActionResult Register(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("error send model");
            }
            var user = _context.Users.FirstOrDefault(a => a.Email == userModel.Email);
            if (user != null)
            {
                ViewBag.Message = "Пользователь с таким email уже существует";
                return View(userModel);
            }
            var newUser = new User()
            {
                Name = userModel.Name,
                Email = userModel.Email,
                Cart = new Cart(),
                Surname = userModel.Surname,
                Password = GetHash(userModel.Password),
                Role = _context.Roles.FirstOrDefault(a=>a.Name  == "user")!
            };
            _context.Users.Add(newUser);
            _context.SaveChanges();

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, newUser.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, newUser.Role.Name)
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            HttpContext.SignInAsync(claimsPrincipal);

            //
            return Redirect("/");
        }

        [Route("/accessdenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [Route("/Logout")]
        [Authorize(Roles = "user, admin")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpPost]
        [Authorize(Roles ="admin, user")]
        public IActionResult Info()
        {
            var user = _context.Users.FirstOrDefault(a => a.Email == HttpContext.User.Identity!.Name);
            if (user == null)
                return BadRequest("Auth error");

            return Json(new { name = user.Name, surname = user.Surname});
        }

        public static string GetHash(string password)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
            }
        }
    }
}
