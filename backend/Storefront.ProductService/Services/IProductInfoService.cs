using Storefront.ProductService.Entities;
using Storefront.ProductService.Models.Request;
using Storefront.ProductService.Models.Response;

namespace Storefront.ProductService.Services
{
    public interface IProductInfoService
    {
        Task<List<FlashSale>> AddFlashRangeSaleAsync(List<FlashSaleRequestModel> flashSales);
        Task<bool> DeleteFlashSaleAsync(string flashSaleId);
        Task<List<ProductAddResponse>> GetActiveFlashSalesAsync();

        Task<List<FlashSale>> GetActiveFlashSalesInfoAsync();
        Task<List<FlashSale>> GetFlashSalesByProductAsync(string productId);
        Task<List<string>> GetProductCategories();
        Task<List<ProductAddResponse>> GetProductsByCategoryAsync(string category);
        Task<List<ProductAddResponse>> GetProductsByIds(GetMultipleProductsByIdsRequest request);
        Task<List<ProductAddResponse>> GetRecentProductsAsync();

        Task<List<FlashSale>> GetAllFlashSalesInfoAsync();

        Task<List<string>> GetBrands();
    }
}
