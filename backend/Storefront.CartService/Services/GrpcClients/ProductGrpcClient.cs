namespace Storefront.CartService.Services.GrpcClients;

using Grpc.Core;
using Storefront.ProductService.Protos;

public class ProductGrpcClient : IProductGrpcClient
{
    private readonly ProductRpc.ProductRpcClient _client;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<ProductGrpcClient> _logger;

    public ProductGrpcClient(ProductRpc.ProductRpcClient client, IHttpContextAccessor httpContextAccessor, ILogger<ProductGrpcClient> logger)
    {
        _client = client;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<ProductResponse> GetProductByIdAsync(string productId)
    {
        try
        {
            var authHeaderValue = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            var headers = new Metadata
            {
                { "Authorization", authHeaderValue ?? throw new UnauthorizedAccessException("User has partial permission to access the resources") }
            };
            var request = new GetProductRequest { Id = productId };
            var response = await _client.GetProductByIdAsync(request, headers);
            return response;
        }
        catch (RpcException ex)
        {
            _logger.LogError(ex, "gRPC call failed: {Status}", ex.Status);
            throw;
        }
    }
}
