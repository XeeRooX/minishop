﻿namespace minishop.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int Count { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int CartId { get; set; }
        public Cart Cart { get; set; } = null!;
    }
}
