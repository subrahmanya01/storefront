namespace Storefront.OrderAndShippingService.Entities
{
    public class TaxRate : EntityBase
    {
        public string? Country { get; set; }
        public string? State { get; set; }
        public decimal Rate { get; set; }
        public string? Category { get; set; }
    }
}
