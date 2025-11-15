namespace Storefront.ProductService.Models.Request
{
    public class AddRatingRequest
    {
        public string ProductId { get; set; } = null!;
        public int Rating { get; set; } // 1 to 5
        public string? Comment { get; set; }
    }
}
