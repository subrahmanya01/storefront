using Storefront.OrderAndShippingService.Entities.Enums;

namespace Storefront.OrderAndShippingService.Entities
{
    public class ShippingInfo : EntityBase
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string Carrier { get; set; } = default!;
        public string TrackingNumber { get; set; } = default!;
        public DateTime? ShippedDate { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public ShippingStatus Status { get; set; } = ShippingStatus.Preparing;
    }
}
