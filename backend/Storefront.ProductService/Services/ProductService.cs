using Storefront.ProductService.Entities;
using Storefront.ProductService.Infrastructure.Repository;
using Storefront.ProductService.Models.Response;

namespace Storefront.ProductService.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> IsAnyProductWithSameNameExist(string productName, string? id = null) {
            return await _productRepository.ProductExistsAsync(productName, id);
        }
        public Task<List<Product>> GetAllAsync() => _productRepository.GetAllAsync();

        public Task<Product?> GetByIdAsync(string id) => _productRepository.GetByIdAsync(id);
        public async Task<PaginatedResult<Product>> GetPaginatedProductsAsync(int pageNumber, int pageSize)
        {
            var products = await _productRepository.GetPaginatedProductsAsync(pageNumber, pageSize);
            var totalCount = await _productRepository.GetTotalCountAsync();
            return new PaginatedResult<Product>(products, totalCount, pageSize, pageNumber);
        }

        public Task CreateAsync(Product product) => _productRepository.CreateAsync(product);

        public Task UpdateAsync(string id, Product product) => _productRepository.UpdateAsync(id, product);

        public Task DeleteAsync(string id) => _productRepository.DeleteAsync(id);

        public Task<List<ProductVariant>> GetVariantsAsync(string productId) =>
                _productRepository.GetVariantsAsync(productId);

        public Task<ProductVariant?> GetVariantByIdAsync(string productId, string variantId) =>
            _productRepository.GetVariantByIdAsync(productId, variantId);

        public Task AddVariantAsync(string productId, ProductVariant variant) =>
            _productRepository.AddVariantAsync(productId, variant);

        public Task UpdateVariantAsync(string productId, string variantId, ProductVariant variant) =>
            _productRepository.UpdateVariantAsync(productId, variantId, variant);

        public Task DeleteVariantAsync(string productId, string variantId) =>
            _productRepository.DeleteVariantAsync(productId, variantId);
    }
}
