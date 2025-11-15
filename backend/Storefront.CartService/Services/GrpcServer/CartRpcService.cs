using AutoMapper;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Storefront.CartService.Protos;

namespace Storefront.CartService.Services.GrpcServer
{
    public class CartRpcService : CartRpc.CartRpcBase
    {
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;
        private readonly ILogger<CartRpcService> _logger;
        public CartRpcService(ICartService cartService, IMapper mapper, ILogger<CartRpcService> logger)
        {
            _cartService = cartService;
            _mapper = mapper;
            _logger = logger;
        }

        [Authorize]
        public override async Task<CartRpcResponse> GetCartById(GetCartRpcRequest request, ServerCallContext context)
        {
            _logger.LogInformation("gRPC request received: GetCartById({ProductId})", request.CartId);
            Guid.TryParse(request.CartId, out Guid cartId);
            if (cartId == null)
            {
                throw new ArgumentException("Invalid cart id");
            }
            var cartInfo = await _cartService.GetCartById(cartId);

            if (!cartInfo.IsSuccess)
            {
                _logger.LogWarning("Cart not found: {cartId}", request.CartId);
                throw new RpcException(new Status(StatusCode.NotFound, "Cart not found"));
            }
            var result = _mapper.Map<CartRpcResponse>(cartInfo.Data);
            return result;
        }
    }
}
