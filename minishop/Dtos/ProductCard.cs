namespace minishop.Dtos
{
    public class ProductCard
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public double Price { get; set; }
        public bool InCart { get; set; }
    }
}
