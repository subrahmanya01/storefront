using Storefront.OrderAndShippingService.Entities;
using Storefront.OrderAndShippingService.Entities.Enums;

namespace Storefront.OrderAndShippingService.Models.Response
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }

        public string OrderNumber { get; set; } = default!;
        public decimal TotalAmount { get; set; }
        public List<OrderItemResponse> OrderItems { get; set; } = new();
        public ShippingAddress ShippingAddress { get; set; } = default!;
        public BillingAddress? BillingAddress { get; set; }
        public OrderStatus Status { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }

    public class OrderItemResponse
    {
        public string ProductId { get; set; }
        public string VariantId { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
