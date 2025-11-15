namespace Storefront.OrderAndShippingService.Models.Request
{
    public class OrderPriceRequest
    {

        public List<ProductInfomation> ProductInfomations { get; set; } = new List<ProductInfomation>();
    
        public string? PostalCode { get; set; }
        public string? CouponCode { get; set; }

        public double SubTotal { get; set; } = 0;
    }

    public class ProductInfomation
    {
        public string? ProductId { get; set; }
        public string? VariantId { get; set; }
        public int Quantity { get; set; }
    }
}
