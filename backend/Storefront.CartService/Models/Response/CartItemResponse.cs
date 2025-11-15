using Storefront.CartService.Entities;

namespace Storefront.CartService.Models.Response
{
    public class CartItemResponse : EntityBase
    {
        public string ProductId { get; set; } = null!;
        public string VariantId { get; set; } = null!;
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
