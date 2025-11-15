using Storefront.OrderAndShippingService.Entities;

namespace Storefront.OrderAndShippingService.Models.Request
{
    public class CreateOrderRequest
    {
        public Guid CartId { get; set; }
        public ShippingAddress ShippingAddress { get; set; } = default!;
        public string? CouponCode { get; set; }
    }
}
