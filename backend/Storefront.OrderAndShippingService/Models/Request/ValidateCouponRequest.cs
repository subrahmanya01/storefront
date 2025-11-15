namespace Storefront.OrderAndShippingService.Models.Request
{
    public class ValidateCouponRequest
    {
        public string Code { get; set; }
        public decimal OrderAmount { get; set; }
        public string? ProductId { get; set; }
        public string? Category { get; set; }
    }

}
