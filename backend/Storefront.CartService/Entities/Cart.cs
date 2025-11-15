using Microsoft.EntityFrameworkCore;

namespace Storefront.CartService.Entities
{
    [Index(nameof(UserId), IsUnique = true)]
    public class Cart : EntityBase
    {
        public Guid UserId { get; set; }

        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
