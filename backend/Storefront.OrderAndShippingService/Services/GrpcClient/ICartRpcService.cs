using Storefront.OrderAndShippingService.Protos;

namespace Storefront.OrderAndShippingService.Services.GrpcServices
{
    public interface ICartRpcService
    {
        Task<CartRpcResponse> GetCartByIdAsync(Guid cartId);
    }
}
