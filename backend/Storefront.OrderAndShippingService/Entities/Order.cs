using Microsoft.AspNetCore.Http;
using Storefront.OrderAndShippingService.Entities.Enums;

namespace Storefront.OrderAndShippingService.Entities
{
    public class Order : EntityBase
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = default!;
        public Guid CustomerId { get; set; }

        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal ShippingFee { get; set; }

        public ShippingAddress ShippingAddress { get; set; } = default!;
        public BillingAddress? BillingAddress { get; set; } 

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        public ShippingInfo? ShippingInfo { get; set; }
    }

}
