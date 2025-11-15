namespace Storefront.OrderAndShippingService.Models.Response
{
    public class OrderPriceResponse
    {
        public double? ShippingFee { get; set; }
        public List<ProductDiscount>? Discounts { get; set; }
        public double? TotalPrice { get; set; }
        public double? Tax { get; set; }
    }

    public class ProductDiscount
    {
        public string? ProductId { get; set; }

        public string? VariantId { get; set; }  
        public double? Discount { get; set; }
    }
}
