namespace Storefront.ProductService.Models.Response
{
    public class RatingResponse
    {
        public string Id { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public int Stars { get; set; }
        public string? Comment { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class RatingInfo
    {
        public string? ProductId { get; set; }

        public int AvgRating { get; set; }
    }
}
