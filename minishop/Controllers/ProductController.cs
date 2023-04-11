using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using minishop.Dtos;
using minishop.Models;
using System.Data;


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

            return View();
        }

        [HttpPost]
        public IActionResult LoadFromTo([FromBody] LoadProductData prData)
        {
            int countProducts = 8;
            int userId = 1;


            if (ModelState.IsValid != true)
            {
                return BadRequest("Bad content");
            }

            int categoryNumber = 0;

            if (prData.Category == "mech")
            {
                categoryNumber = 2;
            }
            else if (prData.Category == "elec")
            {
                categoryNumber = 1;

            }
            else if (prData.Category == "smart")
            {
                categoryNumber = 3;
            }

                IQueryable<Product> products;

            if (prData.LastProductId == 0)
            {
                if (prData.DescendingPrice)
                {               
                    products = from p in context.Products
                               where p.Price >= prData.PriceFrom && p.Price <= prData.PriceTo
                               where p.Id != prData.LastProductId
                               orderby p.Price descending
                               select p;
                }
                else
                {
                    products = from p in context.Products
                               where p.Price >= prData.PriceFrom && p.Price <= prData.PriceTo
                               where p.Id != prData.LastProductId
                               orderby p.Price ascending
                               select p;
                }
                
            }
            else
            {
                Product? lastProduct = context.Products.Find(prData.LastProductId);
                if (lastProduct == null)
                {
                    return BadRequest("Product with Id=LastProductId not found");
                }
                double lastProductPrice = lastProduct.Price;
                if (prData.DescendingPrice)
                {
                    products = from p in context.Products
                               where p.Price >= prData.PriceFrom && p.Price <= lastProductPrice                              
                               where p.Id != prData.LastProductId
                               orderby p.Price descending
                               select p;
                }
                else
                {
                    products = from p in context.Products
                               where p.Price >= lastProductPrice && p.Price <= prData.PriceTo
                               where p.Id != prData.LastProductId
                               orderby p.Price ascending
                               select p;
                }
                
            }

            if (categoryNumber != 0)
            {
                products = from p in products
                           where p.TypeProductId == categoryNumber
                           select p;
            }

            products = products.Take(countProducts + 1);
            int countLoadedProducts = products.Count();

            products = products.Take(countProducts);

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

            var user = context.Users.Include(u => u.Cart!.CartItems).FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                foreach (var pr in resProducts)
                {
                    if (user.Cart!.CartItems.FirstOrDefault(ci => ci.ProductId == pr.Id) != null)
                    {
                        pr.InCart = true;
                    }
                }

            }

            bool lastPage = false;
            if (countLoadedProducts != countProducts + 1)
            {
                lastPage = true;
            }
            return Json(new
            {
                lastPage = lastPage,
                products = resProducts
            }); ;
        }

        [HttpPost]
        public IActionResult LoadByPrice([FromBody] LoadProductData prData)
        {
            int countProducts = 8;
            int userId = 1;


            if (ModelState.IsValid != true)
            {
                return BadRequest("Bad content");
            }

            int categoryNumber = 0;

            if (prData.Category == "mech")
            {
                categoryNumber = 2;
            }
            else if (prData.Category == "elec")
            {
                categoryNumber = 1;

            }
            else if (prData.Category == "smart")
            {
                categoryNumber = 3;
            }

            IQueryable<Product> products;

            if (prData.LastProductId == 0)
            {
                if (prData.DescendingPrice)
                {
                    products = from p in context.Products
                               orderby p.Price descending
                               select p;
                }
                else
                {
                    products = from p in context.Products
                               orderby p.Price ascending
                               select p;
                }
            }
            else
            {
                Product? lastProduct = context.Products.Find(prData.LastProductId);
                if (lastProduct == null)
                {
                    return BadRequest("Product with Id=LastProductId not found");
                }
                double lastProductPrice = lastProduct.Price;

                if (prData.DescendingPrice)
                {
                    products = from p in context.Products
                               where p.Price <= lastProductPrice
                               where p.Id != prData.LastProductId
                               orderby p.Price descending
                               select p;
                }
                else
                {
                    products = from p in context.Products
                               where p.Price >= lastProductPrice
                               where p.Id != prData.LastProductId
                               orderby p.Price ascending
                               select p;
                }
                
            }

            if (categoryNumber != 0)
            {
                products = from p in products
                           where p.TypeProductId == categoryNumber
                           select p;
            }

            products = products.Take(countProducts + 1);
            int countLoadedProducts = products.Count();

            products = products.Take(countProducts);

            

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

            var user = context.Users.Include(u => u.Cart!.CartItems).FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                foreach (var pr in resProducts)
                {
                    if (user.Cart!.CartItems.FirstOrDefault(ci => ci.ProductId == pr.Id) != null)
                    {
                        pr.InCart = true;
                    }
                }

            }



            bool lastPage = false;
            if (countLoadedProducts != countProducts + 1)
            {
                lastPage = true;
            }
            return Json(new
            {
                lastPage = lastPage,
                products = resProducts
            }); ;
        }

        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            ViewBag.Types = new SelectList(context.TypeProducts, "Id", "Name");
            return View();
        }


        [HttpPost]
        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
            var product = context.Products.Find(id);
            if (product == null)
                return BadRequest();

            if (HttpContext.User.Identity!.IsAuthenticated)
            {
                var user = context.Users.Include(a => a.Cart.CartItems).FirstOrDefault(a => a.Email == HttpContext.User.Identity!.Name);

                if (user == null)
                    return BadRequest();

                if (id == 0)
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
            else
            {
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
                return View(productModel);
            }
        }

        [Authorize(Roles = "admin")]
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
