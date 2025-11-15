using Storefront.ProductService.Protos;

namespace Storefront.CartService.Services.GrpcClients
{
    public interface IProductGrpcClient
    {
        Task<ProductResponse> GetProductByIdAsync(string productId);
    }
}
