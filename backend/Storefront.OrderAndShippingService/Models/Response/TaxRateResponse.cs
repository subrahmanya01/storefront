namespace Storefront.OrderAndShippingService.Models.Response
{
    public class TaxRateResponse
    {
        public Guid Id { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public decimal Rate { get; set; }
        public string? Category { get; set; }
    }
}
