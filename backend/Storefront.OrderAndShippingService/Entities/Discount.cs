namespace Storefront.OrderAndShippingService.Entities
{
    public class Discount : EntityBase
    {
        public string? Code { get; set; }
        public decimal Percentage { get; set; }
        public decimal? MinOrderAmount { get; set; }
        public string? Category { get; set; }
        public string? ProductId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
    }

}
