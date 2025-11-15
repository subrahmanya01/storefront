using Storefront.ProductService.Protos;

namespace Storefront.ProductService.Services.GrpcClient
{
    public interface IOrderRpcService
    {
        Task<GetAllDiscountsResponse> GetAllDiscounts();
        Task<GetAllShippingChargesResponse> GetAllShippingCharges();
    }
}
