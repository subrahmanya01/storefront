using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Storefront.ProductService.Protos;

namespace Storefront.ProductService.Services.GrpcClient
{
    public class OrderRpcService : IOrderRpcService
    {

        private readonly OrderRpc.OrderRpcClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<OrderRpcService> _logger;

        public OrderRpcService(OrderRpc.OrderRpcClient client, IHttpContextAccessor httpContextAccessor, ILogger<OrderRpcService> logger)
        {
            _client = client;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<GetAllDiscountsResponse> GetAllDiscounts()
        {
            try
            {
                var authHeaderValue = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
                var headers = new Metadata
            {
                { "Authorization", authHeaderValue ?? throw new UnauthorizedAccessException("User has partial permission to access the resources") }
            };
                var response = await _client.GetAllDiscountsAsync(new Empty(), headers);
                return response;
            }
            catch (RpcException ex)
            {
                _logger.LogError(ex, "gRPC call failed: {Status}", ex.Status);
                throw;
            }
        }

        public async Task<GetAllShippingChargesResponse> GetAllShippingCharges()
        {
            try
            {
                var authHeaderValue = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
                var headers = new Metadata
            {
                { "Authorization", authHeaderValue ?? throw new UnauthorizedAccessException("User has partial permission to access the resources") }
            };
                var response = await _client.GetAllShippingChargesAsync(new Empty(), headers);
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
