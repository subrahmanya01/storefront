namespace Storefront.OrderAndShippingService.Models.Request
{
    public class TaxRateRequest
    {
        public string? Country { get; set; }
        public string? State { get; set; }
        public decimal Rate { get; set; }
        public string? Category { get; set; }
    }
}
