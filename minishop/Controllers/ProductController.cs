using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using minishop.Dtos;
using minishop.Models;

namespace minishop.Controllers
{
    public class ProductController : Controller
    {

        private readonly ApplicationDbContext context = null!;
        IWebHostEnvironment _appEnvironment;
        public ProductController(ApplicationDbContext _context, IWebHostEnvironment appEnvironment)
        {
            context = _context;
            _appEnvironment = appEnvironment;   
        }

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
            ViewBag.Types = new SelectList(context.TypeProducts, "Id", "Name"); 
            return View();
        }


        [HttpPost]
        public IActionResult Create(ProductModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Ошибка загрузки";
                return  View();
            }
            Product product = new Product() { Name = model.Name, Price = model.Price, Description = model.Description, 
                TypeProduct = context.TypeProducts.Find(model.TypeProductId) };

            context.Products.Add(product);
            context.SaveChanges();

            string path = "/Files/" +$"{product.Id}" +model.Foto.FileName.Replace(model.Foto.Name,"");//model.Foto.FileName;
            // сохраняем файл в папку Files в каталоге wwwroot
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                model.Foto.CopyTo(fileStream);
            }


            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }
    }
}
