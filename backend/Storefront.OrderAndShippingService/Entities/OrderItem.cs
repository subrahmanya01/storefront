namespace Storefront.OrderAndShippingService.Entities
{
    public class OrderItem : EntityBase
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string? ProductId { get; set; }
        public string? VariantId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total => UnitPrice * Quantity;
    }
}
