using Storefront.ProductService.Entities;
using Storefront.ProductService.Protos;

namespace Storefront.ProductService.Models.Response
{
    public class ProductAddResponse : Product
    {
        public int Rating { get; set; }
        public List<DiscountRpcResponse>? Discounts { get; set; } = new List<DiscountRpcResponse>();
        public List<ShippingChargeRpcResponse> ShippingCharges { get; set;} = new List<ShippingChargeRpcResponse>();
    }
}
