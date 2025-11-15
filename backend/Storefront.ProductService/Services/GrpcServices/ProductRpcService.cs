using AutoMapper;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Storefront.ProductService.Models.Request;
using Storefront.ProductService.Protos;

namespace Storefront.ProductService.Services.GrpcServices
{
    public class ProductRpcService : ProductRpc.ProductRpcBase
    {
        private readonly IProductService _productService;
        private readonly IProductInfoService _productInfoService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductRpcService> _logger;
        public ProductRpcService(IProductService productService, IMapper mapper, ILogger<ProductRpcService> logger, IProductInfoService productInfoService)
        {
            _productService = productService;
            _mapper = mapper;
            _logger = logger;
            _productInfoService = productInfoService;
        }

        [Authorize]
        public override async Task<ProductResponse> GetProductById(GetProductRequest request, ServerCallContext context)
        {
            _logger.LogInformation("gRPC request received: GetProductById({ProductId})", request.Id);

            var productInfo = await _productService.GetByIdAsync(request.Id);

            if (productInfo == null)
            {
                _logger.LogWarning("Product not found: {ProductId}", request.Id);
                throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));
            }

            return _mapper.Map<ProductResponse>(productInfo);
        }

        [Authorize]
        public override async Task<GetProductsByIdsResponse> GetProductsByIds(GetProductsByIdsRequest request, ServerCallContext context)
        {
            var productRequest = new GetMultipleProductsByIdsRequest {
                ProductIds = request.Ids.ToList(),
            };
            var productInfo = await _productInfoService.GetProductsByIds(productRequest);

            if (productInfo == null)
            {
                _logger.LogWarning("Product info not found for requested ids");
                throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));
            }
            var mappedProducts = _mapper.Map<List<ProductResponse>>(productInfo);
            var response = new GetProductsByIdsResponse();
            response.Products.AddRange(mappedProducts);
            return response;
        }
    }
}
