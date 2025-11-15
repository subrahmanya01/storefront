namespace Storefront.OrderAndShippingService.Entities
{
    public class ShippingCharge : EntityBase
    {
        public string? Country { get; set; }
        public string? Region { get; set; }
        public string? ProductId { get; set; }
        public decimal MinOrderAmount { get; set; }
        public decimal? MaxOrderAmount { get; set; } 
        public decimal ShippingFeePerKm { get; set; }
        public bool IsFree { get; set; } 
        public string? Carrier { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }
}
