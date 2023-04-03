namespace minishop.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Price { get; set; }

        public List<CartItem> CartItem { get; set; } = new();
        public int TypeProductId { get; set; }
        public TypeProduct TypeProduct { get; set; } = null!;
    }
}
