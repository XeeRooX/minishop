namespace minishop.Models
{
    public class InitializeDb
    {
        IConfiguration config;
        ApplicationDbContext context;
        public InitializeDb(IConfiguration conf, ApplicationDbContext context)
        {
            this.config = conf;
            this.context = context;
        }

        public void Initialize()
        {
            if (context.Roles.Count() == 0)
            {
                context.Roles.AddRange(
                    new Role() { Name = "admin" },
                    new Role() { Name = "user" }
                    );
                context.SaveChanges();
            }

            if (context.Users.Count() == 0)
            {
                string email = config["admin:email"];
                string password = config["admin:password"];

                var role = context.Roles.FirstOrDefault(r=>r.Name == "admin");

                var user = new User() { Email = email, Name = "Admin", Role = role!, Password = minishop.Controllers.UserController.GetHash(password), Surname = "Admin", Cart = new Cart() };
                context.Add(user);
                context.SaveChanges();
            }

            if (context.TypeProducts.Count() == 0)
            {
                context.TypeProducts.AddRange(
                    new TypeProduct() { Id = 1, Name = "Электронные часы" },
                    new TypeProduct() { Id = 2, Name = "Механические часы" },
                    new TypeProduct() { Id = 3, Name = "Смарт-часы" }
                    );

                context.SaveChanges();
            }

        }
    }
}
