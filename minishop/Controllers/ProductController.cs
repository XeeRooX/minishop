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
            int countCards = 8; 
            ViewBag.Category = category;

            IQueryable<Product> products;
            if (category == "mech")
            {
                products = context.Products.Where(p => p.TypeProductId == 2);
            }
            else if (category == "elec")
            {
                products = context.Products.Where(p => p.TypeProductId == 1);

            }
            else if (category == "smart")
            {
                products = context.Products.Where(p => p.TypeProductId == 3);
            }
            else
            {
                products = context.Products;
            }

            products = products.Take(countCards);

            var resProducts = new List<ProductCard>();

            foreach (var pr in products)
            {
                resProducts.Add(new ProductCard()
                {
                    Id = pr.Id,
                    Price = pr.Price,
                    Title = pr.Name
                });
            }
            
            return View(resProducts);
        }

        [HttpPost]
        public IActionResult LoadProducts([FromBody]LoadProductData prData)
        {
            if (ModelState.IsValid != true)
            {
                return BadRequest("Bad content");
            }
           
            var products = from p in context.Products
                           where p.Price >= prData.PriceFrom && p.Price <= prData.PriceTo
                           select p;              

            return Json(prData);
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
