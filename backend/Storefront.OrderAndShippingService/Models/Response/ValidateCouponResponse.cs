namespace Storefront.OrderAndShippingService.Models.Response
{
    public class ValidateCouponResponse
    {
        public bool IsValid { get; set; }
        public decimal DiscountPercentage { get; set; }
        public string Message { get; set; }
    }

}
