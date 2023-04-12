using System.ComponentModel.DataAnnotations;

namespace minishop.Dtos
{
    public class LoadProductData
    {
        public double PriceFrom { get; set; }
        public double PriceTo { get; set; }
        public int LastProductId { get; set; }
        public bool DescendingPrice { get; set; }
        public string? Category { get; set; }
    }
}
