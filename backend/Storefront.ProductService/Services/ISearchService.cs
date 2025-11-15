using Storefront.ProductService.Entities;
using Storefront.ProductService.Models.Request;

namespace Storefront.ProductService.Services
{
    public interface ISearchService
    {
        Task<List<Product>> GetSearchResult(SearchRequest request);
    }
}
