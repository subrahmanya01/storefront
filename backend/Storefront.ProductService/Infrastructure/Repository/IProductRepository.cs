using MongoDB.Driver;
using Storefront.ProductService.Entities;
using System.Linq.Expressions;

namespace Storefront.ProductService.Infrastructure.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<List<Product>> GetAsync(Expression<Func<Product, bool>> expression);
        Task<List<Product>> GetAsync(FilterDefinition<Product> filter);
        Task<Product?> GetByIdAsync(string id);
        Task<Product> CreateAsync(Product product);
        Task UpdateAsync(string id, Product product);
        Task DeleteAsync(string id);
        Task<List<Product>> GetPaginatedProductsAsync(int pageNumber, int pageSize);
        Task<int> GetTotalCountAsync();
        Task<bool> ProductExistsAsync(string productName, string? id=null);
        Task<List<ProductVariant>> GetVariantsAsync(string productId);
        Task<ProductVariant?> GetVariantByIdAsync(string productId, string variantId);
        Task AddVariantAsync(string productId, ProductVariant variant);
        Task UpdateVariantAsync(string productId, string variantId, ProductVariant variant);
        Task DeleteVariantAsync(string productId, string variantId);
    }
}
