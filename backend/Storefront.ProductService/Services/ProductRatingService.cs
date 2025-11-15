using AutoMapper;
using MongoDB.Driver;
using Storefront.ProductService.Entities;
using Storefront.ProductService.Infrastructure.Repository;
using Storefront.ProductService.Models.Response;

namespace Storefront.ProductService.Services
{
    public class ProductRatingService : IProductRatingService
    {
        private readonly IProductRatingRepository _repository;
        private readonly IMapper _mapper;

        public ProductRatingService(IProductRatingRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<RatingResponse>> GetRatingsAsync(string productId)
        {
            var ratings = await _repository.GetRatingsByProductIdAsync(productId);
            return _mapper.Map<List<RatingResponse>>(ratings);
        }

        public async Task<RatingResponse> RateProductAsync(string userId, string productId, int stars, string? comment)
        {
            var rating = new Entities.ProductRating
            {
                ProductId = productId,
                UserId = userId,
                Stars = stars,
                Comment = comment,
                Timestamp = DateTime.UtcNow
            };

            var saved = await _repository.AddOrUpdateRatingAsync(rating);
            return _mapper.Map<RatingResponse>(saved);
        }

        public async Task<bool> DeleteRatingAsync(string ratingId)
        {
            return await _repository.DeleteRatingAsync(ratingId);
        }

        public async Task<List<RatingInfo>> GetRatingsByProductIdsAsync(List<string> productIds)
        {
            var filter = Builders<ProductRating>.Filter.In(p => p.ProductId, productIds);
            var ratings = await _repository.GetProductRatingAsync(filter);

            var response = ratings.GroupBy(x => x.ProductId)
                .Select(x => new RatingInfo
                {
                    ProductId = x.Key,
                    AvgRating = (int)Math.Round(x.Average(x => x.Stars))
                }
            ).ToList();
            return response;
        }

        public async Task<List<RatingResponse>> GetUserRatingForProducts(string userId)
        {
            var builder = Builders<ProductRating>.Filter;
            var filter = builder.Eq(p => p.UserId, userId.ToString());
            var ratings = await _repository.GetProductRatingAsync(filter);
            var mappedRatingResponse = _mapper.Map<List<RatingResponse>>(ratings);
            return mappedRatingResponse;
        }
    }

}
