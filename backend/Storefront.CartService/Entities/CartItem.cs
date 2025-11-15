using Microsoft.EntityFrameworkCore;

namespace Storefront.CartService.Entities
{
    [Index(nameof(CartId))]
    [Index(nameof(CartId), nameof(ProductId), nameof(VariantId), IsUnique = true)]
    public class CartItem : EntityBase
    {
        public Guid CartId { get; set; }
        public Cart? Cart { get; set; }
        public string ProductId { get; set; } = null!;
        public string VariantId { get; set; } = null!; 
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
