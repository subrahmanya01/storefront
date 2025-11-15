using Grpc.Core;
using Storefront.OrderAndShippingService.Protos;

namespace Storefront.OrderAndShippingService.Services.GrpcClient
{
    public class ProductRpcService : IProductRpcService
    {
        private readonly ProductRpc.ProductRpcClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ProductRpcService> _logger;

        public ProductRpcService(ProductRpc.ProductRpcClient client, IHttpContextAccessor httpContextAccessor, ILogger<ProductRpcService> logger)
        {
            _client = client;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<List<ProductResponse>> GetProductsById(List<string> ids)
        {
            try
            {
                var authHeaderValue = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
                var headers = new Metadata
            {
                { "Authorization", authHeaderValue ?? throw new UnauthorizedAccessException("User has partial permission to access the resources") }
            };
                var request = new GetProductsByIdsRequest();
                request.Ids.AddRange(ids);
                var response = await _client.GetProductsByIdsAsync(request, headers);
                return response.Products.ToList();
            }
            catch (RpcException ex)
            {
                _logger.LogError(ex, "gRPC call failed: {Status}", ex.Status);
                throw;
            }
        }
    }
}
