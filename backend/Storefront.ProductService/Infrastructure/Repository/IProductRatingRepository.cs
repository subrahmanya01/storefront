using DnsClient;
using MongoDB.Driver;
using Storefront.ProductService.Entities;

namespace Storefront.ProductService.Infrastructure.Repository
{
    public interface IProductRatingRepository
    {
        Task<List<ProductRating>> GetRatingsByProductIdAsync(string productId);
        Task<List<ProductRating>> GetProductRatingAsync(FilterDefinition<ProductRating> filter);
        Task<ProductRating?> GetRatingByUserAndProductAsync(string userId, string productId);
        Task<ProductRating> AddOrUpdateRatingAsync(ProductRating rating);
        Task<bool> DeleteRatingAsync(string ratingId);
    }

}
