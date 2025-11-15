using DnsClient;
using Storefront.ProductService.Models.Response;

namespace Storefront.ProductService.Services
{
    public interface IProductRatingService
    {
        Task<List<RatingResponse>> GetRatingsAsync(string productId);
        Task<List<RatingResponse>> GetUserRatingForProducts(string userId);
        Task<List<RatingInfo>> GetRatingsByProductIdsAsync(List<string> productIds);
        Task<RatingResponse> RateProductAsync(string userId, string productId, int stars, string? comment);
        Task<bool> DeleteRatingAsync(string ratingId);
    }
}
