using minishop.Models;
using Microsoft.AspNetCore.Http;

namespace minishop.Dtos
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Price { get; set; }
        public int TypeProductId { get; set; }

        public bool InCart { get; set; }

        public int Count { get; set; }
        public IFormFile Foto { get; set; }
    }
}
