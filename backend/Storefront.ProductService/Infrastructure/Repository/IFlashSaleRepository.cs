using MongoDB.Driver;
using Storefront.ProductService.Entities;
using System.Linq.Expressions;

namespace Storefront.ProductService.Infrastructure.Repository
{
    public interface IFlashSaleRepository
    {
        Task<List<FlashSale>> GetAllAsync();
        Task<List<FlashSale>> GetAsync(Expression<Func<FlashSale, bool>> expression);
        Task<List<FlashSale>> GetAsyncByFilter(FilterDefinition<FlashSale> filter);
        Task<FlashSale?> GetByIdAsync(string id);
        Task<List<FlashSale>> InsertRangeAsync(List<FlashSale> flashSale);
        Task<FlashSale> InsertAsync(FlashSale flashSale);
        Task<bool> UpdateAsync(string id, FlashSale updatedSale);
        Task<bool> DeleteAsync(string id);
        FilterDefinitionBuilder<FlashSale> FilterBuilder { get; }
    }

}
