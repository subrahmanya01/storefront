using DnsClient;
using MongoDB.Driver;
using Storefront.ProductService.Entities;

namespace Storefront.ProductService.Infrastructure.Repository
{
    public class ProductRatingRepository : IProductRatingRepository
    {
        private readonly IMongoCollection<ProductRating> _ratings;

        public ProductRatingRepository(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MongoDB:ConnectionString"]);
            var database = client.GetDatabase(configuration["MongoDB:Database"]);
            _ratings = database.GetCollection<ProductRating>("Ratings");
        }

        public async Task<List<ProductRating>> GetProductRatingAsync(FilterDefinition<ProductRating> filter)
        {
            return await _ratings.Find(filter).ToListAsync();
        }

        public async Task<List<ProductRating>> GetRatingsByProductIdAsync(string productId)
        {
            return await _ratings.Find(r => r.ProductId == productId).ToListAsync();
        }

        public async Task<ProductRating?> GetRatingByUserAndProductAsync(string userId, string productId)
        {
            return await _ratings.Find(r => r.UserId == userId && r.ProductId == productId).FirstOrDefaultAsync();
        }

        public async Task<ProductRating> AddOrUpdateRatingAsync(ProductRating rating)
        {
            var existing = await GetRatingByUserAndProductAsync(rating.UserId, rating.ProductId);
            if (existing != null)
            {
                rating.Id = existing.Id;
                await _ratings.ReplaceOneAsync(r => r.Id == rating.Id, rating);
            }
            else
            {
                await _ratings.InsertOneAsync(rating);
            }

            return rating;
        }

        public async Task<bool> DeleteRatingAsync(string ratingId)
        {
            var result = await _ratings.DeleteOneAsync(r => r.Id == ratingId);
            return result.DeletedCount > 0;
        }
    }
}
