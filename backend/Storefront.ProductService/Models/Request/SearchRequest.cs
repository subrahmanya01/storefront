namespace Storefront.ProductService.Models.Request
{
    public class SearchRequest
    {
        public string? Keyword { get; set; }

        public SearchFilters? Filters { get; set; }
    }

    public class SearchFilters
    {
        public string? Category { get; set; }
        public string? Brand { get; set; }
        public double? PriceStart { get; set; }
        public double? PriceEnd { get; set; }
    }
}
