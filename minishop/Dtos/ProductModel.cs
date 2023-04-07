using minishop.Models;

namespace minishop.Dtos
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Price { get; set; }
        public int TypeProductId { get; set; }

        IFormFile Foto { get; set; } = null!;
    }
}
