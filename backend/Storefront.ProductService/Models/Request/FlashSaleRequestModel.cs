namespace Storefront.ProductService.Models.Request
{
    public class FlashSaleRequestModel
    {
        public string? ProductId { get; set; } = null!;
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }
    }
}
