namespace Storefront.ProductService.Models.Request
{
    public class GetMultipleProductsByIdsRequest
    {
        public List<string>? ProductIds { get; set; }
    }
}
