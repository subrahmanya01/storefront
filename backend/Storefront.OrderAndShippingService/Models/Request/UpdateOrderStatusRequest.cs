using Storefront.OrderAndShippingService.Entities.Enums;

namespace Storefront.OrderAndShippingService.Models.Request
{
    public class UpdateOrderStatusRequest
    {
        public Guid OrderId { get; set; }

        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
    }
}
