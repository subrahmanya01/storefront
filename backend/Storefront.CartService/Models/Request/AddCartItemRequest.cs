namespace Storefront.CartService.Models.Request
{
    public class AddCartItemRequest
    {
        public string ProductId { get; set; } = string.Empty;
        public string ProductVariantId { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
