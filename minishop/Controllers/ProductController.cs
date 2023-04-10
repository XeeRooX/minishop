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
            var user = context.Users.Include(a => a.Cart.CartItems).FirstOrDefault(a => a.Id == 1);

            if (user == null)
                return BadRequest();

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
                TypeProductId = product.TypeProductId,
                InCart = false,
                Count = 1
            };

            foreach (var a in user.Cart!.CartItems)
            {
                if (a.Product == product)
                {
                    productModel.InCart = true;
                    productModel.Count = a.Count;
                }

            }
          
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
