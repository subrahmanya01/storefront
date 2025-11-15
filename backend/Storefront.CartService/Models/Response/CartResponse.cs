using Storefront.CartService.Entities;

namespace Storefront.CartService.Models.Response
{
    public class CartResponse : EntityBase
    {
        public Guid UserId { get; set; }

        public ICollection<CartItemResponse>? Items { get; set; } = new List<CartItemResponse>();
    }
}
