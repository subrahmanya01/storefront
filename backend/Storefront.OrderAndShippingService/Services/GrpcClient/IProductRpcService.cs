using Storefront.OrderAndShippingService.Protos;

namespace Storefront.OrderAndShippingService.Services.GrpcClient
{
    public interface IProductRpcService
    {
        Task<List<ProductResponse>> GetProductsById(List<string> ids);
    }
}
