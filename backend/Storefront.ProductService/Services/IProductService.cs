using Storefront.ProductService.Entities;
using Storefront.ProductService.Models.Response;

namespace Storefront.ProductService.Services
{
    public interface IProductService
    {
        #region Product Apis
        Task<List<Product>> GetAllAsync();

        Task<Product?> GetByIdAsync(string id);
        Task CreateAsync(Product product);
        Task UpdateAsync(string id, Product product);
        Task DeleteAsync(string id);
        Task<PaginatedResult<Product>> GetPaginatedProductsAsync(int pageNumber, int pageSize);
        Task<bool> IsAnyProductWithSameNameExist(string productName, string? id = null);
        #endregion

        #region Product Varient Apis
        Task<List<ProductVariant>> GetVariantsAsync(string productId);
        Task<ProductVariant?> GetVariantByIdAsync(string productId, string variantId);
        Task AddVariantAsync(string productId, ProductVariant variant);
        Task UpdateVariantAsync(string productId, string variantId, ProductVariant variant);
        Task DeleteVariantAsync(string productId, string variantId);
        #endregion
    }

}
