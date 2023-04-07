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
                return View();
            }
            Product product = new Product()
            {
                Name = model.Name,
                Price = model.Price,
                Description = model.Description,
                TypeProduct = context.TypeProducts.Find(model.TypeProductId)!
            };

            context.Products.Add(product);
            context.SaveChanges();

            string path = "/imgs/products/" + $"{product.Id}" + ".png";

            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                model.Foto.CopyTo(fileStream);
            }


            return Redirect($"/Product/{product.Id}");
        }

        public IActionResult Edit(int id)
        {
            if (id == 0)
                return BadRequest();
            var product = context.Products.Find(id);
            if (product == null)
                return BadRequest();
            var productModel = new ProductModel()
            {
                Name = product.Name,
                Description = product.Description
            ,
                Price = product.Price,
                Id = product.Id,
                TypeProductId = product.TypeProductId
            };
            ViewBag.Types = new SelectList(context.TypeProducts, "Id", "Name");
            return View(productModel);
        }
        [HttpPost]
        public IActionResult Edit(ProductModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Ошибка загрузки";
                return View();
            }
            var product = context.Products.Find(model.Id);
            if (product == null)
            {
                ViewBag.Message = "Ошибка загрузки";
                return View();
            }
            product.Description = model.Description;
            product.Price = model.Price;
            product.Name = model.Name;
            product.TypeProduct = context.TypeProducts.Find(model.TypeProductId)!;

            string path = "/imgs/products/" + $"{product.Id}" + ".png";

            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                model.Foto.CopyTo(fileStream);
            }
            context.SaveChanges();
            return Redirect($"/Product/{product.Id}");
        }
        [Route("/Product/{id:int}")]
        public IActionResult Details(int id)
        {
            if (id == 0)
                return BadRequest();
            var product = context.Products.Find(id);
            if (product == null)
                return BadRequest();
            var productModel = new ProductModel()
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Id = product.Id,
                TypeProductId = product.TypeProductId
            };
            return View(productModel);
        }

        public IActionResult Delete(int id)
        {
            if (id == 0)
                return BadRequest();
            var product = context.Products.Find(id);
            if(product==null)
                return BadRequest();

            string path = _appEnvironment.WebRootPath+ "/imgs/products/" + $"{product.Id}" + ".png";

            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                file.Delete();   
            }
            else
            {  
            }

            context.Products.Remove(product);
            context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
