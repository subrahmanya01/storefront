using MongoDB.Bson;
using MongoDB.Driver;
using Storefront.ProductService.Entities;
using Storefront.ProductService.Infrastructure.Repository;
using Storefront.ProductService.Models.Request;

namespace Storefront.ProductService.Services
{
    public class SearchService : ISearchService
    {
        private readonly IProductRepository _productRepository;
        public SearchService(IProductRepository productRepository) {
            _productRepository = productRepository;
        }

        public async Task<List<Product>> GetSearchResult(SearchRequest request)
        {
            var filterBuilder = Builders<Product>.Filter.Where(x => true); 

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                var keyword = request.Keyword.ToLower();
                filterBuilder &= Builders<Product>.Filter.Or(
                    Builders<Product>.Filter.Regex(x => x.Category, new BsonRegularExpression(keyword, "i")),
                    Builders<Product>.Filter.Regex(x => x.Name, new BsonRegularExpression(keyword, "i")), 
                    Builders<Product>.Filter.Regex(x => x.Description, new BsonRegularExpression(keyword, "i"))
                );
            }

            if (!string.IsNullOrEmpty(request.Filters?.Category))
            {
                var categoryFilter = Builders<Product>.Filter.Eq(x => x.Category, request.Filters.Category);
                filterBuilder &= categoryFilter;
            }

            if (!string.IsNullOrEmpty(request.Filters?.Brand))
            {
                var brandFilter = Builders<Product>.Filter.Eq(x => x.BaseAttributes.GetValueOrDefault("Brand"), request.Filters.Brand);
                filterBuilder &= brandFilter;
            }

            if (request.Filters?.PriceStart.HasValue == true || request.Filters?.PriceEnd.HasValue == true)
            {
                var priceFilter = Builders<Product>.Filter.Where(x =>
                    (request.Filters.PriceStart.HasValue == false || x.Variants.Any(v => v.Price >= request.Filters.PriceStart.Value)) &&
                    (request.Filters.PriceEnd.HasValue == false || x.Variants.Any(v => v.Price <= request.Filters.PriceEnd.Value))
                );
                filterBuilder &= priceFilter;
            }

            return await _productRepository.GetAsync(filterBuilder);
        }

    }
}
