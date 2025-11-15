using Grpc.Core;
using Storefront.OrderAndShippingService.Protos;

namespace Storefront.OrderAndShippingService.Services.GrpcServices
{
    public class CartRpcService : ICartRpcService
    {
        private readonly CartRpc.CartRpcClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CartRpcService> _logger;

        public CartRpcService(CartRpc.CartRpcClient client, IHttpContextAccessor httpContextAccessor, ILogger<CartRpcService> logger)
        {
            _client = client;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<CartRpcResponse> GetCartByIdAsync(Guid cartId)
        {
            try
            {
                var authHeaderValue = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
                var headers = new Metadata
            {
                { "Authorization", authHeaderValue ?? throw new UnauthorizedAccessException("User has partial permission to access the resources") }
            };
                var request = new GetCartRpcRequest { CartId = cartId.ToString() };
                var response = await _client.GetCartByIdAsync(request, headers);
                return response;
            }
            catch (RpcException ex)
            {
                _logger.LogError(ex, "gRPC call failed: {Status}", ex.Status);
                throw;
            }
        }
    }
}
