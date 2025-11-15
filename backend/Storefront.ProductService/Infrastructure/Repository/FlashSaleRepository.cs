using MongoDB.Driver;
using Storefront.ProductService.Entities;
using System.Linq.Expressions;

namespace Storefront.ProductService.Infrastructure.Repository
{
    public class FlashSaleRepository : IFlashSaleRepository
    {
        private readonly IMongoCollection<FlashSale> _flashSale;
        private readonly FilterDefinitionBuilder<FlashSale> _filterBuilder = Builders<FlashSale>.Filter;

        public FlashSaleRepository(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MongoDB:ConnectionString"]);
            var database = client.GetDatabase(configuration["MongoDB:Database"]);
            _flashSale = database.GetCollection<FlashSale>("FlashSale");
        }

        public FilterDefinitionBuilder<FlashSale> FilterBuilder => _filterBuilder;

        public async Task<List<FlashSale>> GetAllAsync()
        {
            return await _flashSale.Find(_filterBuilder.Empty).ToListAsync();
        }

        public async Task<List<FlashSale>> GetAsync(Expression<Func<FlashSale, bool>> expression)
        {
            return await _flashSale.Find(expression).ToListAsync();
        }

        public async Task<List<FlashSale>> GetAsyncByFilter(FilterDefinition<FlashSale> filter)
        {
            return await _flashSale.Find(filter).ToListAsync();
        }

        public async Task<FlashSale?> GetByIdAsync(string id)
        {
            var filter = _filterBuilder.Eq(f => f.Id, id);
            return await _flashSale.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<FlashSale> InsertAsync(FlashSale flashSale)
        {
            flashSale.CreatedAt = DateTime.UtcNow;
            await _flashSale.InsertOneAsync(flashSale);
            return flashSale;
        }

        public async Task<List<FlashSale>> InsertRangeAsync(List<FlashSale> flashSale)
        {
            foreach(var item in flashSale)
            {
                item.CreatedAt = DateTime.UtcNow;
            }
            await _flashSale.InsertManyAsync(flashSale);
            return flashSale;
        }

        public async Task<bool> UpdateAsync(string id, FlashSale updatedSale)
        {
            updatedSale.ModifiedAt = DateTime.UtcNow;
            updatedSale.Id = id;

            var result = await _flashSale.ReplaceOneAsync(
                _filterBuilder.Eq(f => f.Id, id),
                updatedSale);

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _flashSale.DeleteOneAsync(_filterBuilder.Eq(f => f.Id, id));
            return result.DeletedCount > 0;
        }
    }

}
