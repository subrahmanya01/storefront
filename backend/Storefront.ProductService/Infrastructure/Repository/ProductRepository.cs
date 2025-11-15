using MongoDB.Bson;
using MongoDB.Driver;
using Storefront.ProductService.Entities;
using System.Linq.Expressions;

namespace Storefront.ProductService.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _productCollection;

        public ProductRepository(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MongoDB:ConnectionString"]);
            var database = client.GetDatabase(configuration["MongoDB:Database"]);
            _productCollection = database.GetCollection<Product>("Products");
        }

        public async Task<bool> ProductExistsAsync(string productName, string? id = null)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Name, productName);
            var existingProduct = await _productCollection.Find(filter).FirstOrDefaultAsync();
            return existingProduct != null && existingProduct.Id != id;
        }

        public async Task<List<Product>> GetPaginatedProductsAsync(int pageNumber, int pageSize)
        {
            var skip = (pageNumber - 1) * pageSize;
            return await _productCollection
                .Find(_ => true) 
                .Skip(skip)
                .Limit(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return (int)await _productCollection.CountDocumentsAsync(_ => true);
        }

        public async Task<List<Product>> GetAsync(Expression<Func<Product, bool>> expression)
        {
            if(expression == null) throw new ArgumentNullException(nameof(expression));
            return await _productCollection.Find(expression).ToListAsync();
        }

        public async Task<List<Product>> GetAsync(FilterDefinition<Product> filter)
        {
            return await _productCollection.Find(filter).ToListAsync();
        }

        public async Task<List<Product>> GetAllAsync() =>
            await _productCollection.Find(_ => true).ToListAsync();

        public async Task<Product?> GetByIdAsync(string id) =>
            await _productCollection.Find(p => p.Id == id).FirstOrDefaultAsync();

        public async Task<Product> CreateAsync(Product product)
        {
            if (product.Variants == null)
                product.Variants = new List<ProductVariant>();

            if (product.AttributeValues == null)
                product.AttributeValues = new Dictionary<string, HashSet<string>>();

            foreach (var item in product.Variants)
            {
                item.Id = ObjectId.GenerateNewId().ToString();
                item.CreatedAt = DateTime.UtcNow;
                item.ModifiedAt = DateTime.UtcNow;
            }

            UpdateProductVariantAttributeValues(product);
            product.ModifiedAt = DateTime.UtcNow;
            product.CreatedAt = DateTime.UtcNow;
            await _productCollection.InsertOneAsync(product);
            return product;
        }

        public async Task UpdateAsync(string id, Product product)
        {
            UpdateProductVariantAttributeValues(product);
            product.ModifiedAt = DateTime.UtcNow;
            await _productCollection.ReplaceOneAsync(p => p.Id == id, product);
        }

        private void UpdateProductVariantAttributeValues(Product product)
        {
            product.AttributeValues.Clear();
            foreach (var attr in product.Variants!.SelectMany(variant => variant.Attributes))
            {
                if (!product.AttributeValues.ContainsKey(attr.Key))
                    product.AttributeValues[attr.Key] = new HashSet<string>();
                product.AttributeValues[attr.Key].Add(attr.Value);
            }
        }

        public async Task DeleteAsync(string id) =>
            await _productCollection.DeleteOneAsync(p => p.Id == id);

        public async Task AddVariantAsync(string productId, ProductVariant variant)
        {
            var product = await _productCollection.Find(p => p.Id == productId).FirstOrDefaultAsync();
            if (product == null)
                throw new Exception("Product not found");

            if(product.Variants == null)
            {
                product.Variants = new();
            }

            variant.Id = ObjectId.GenerateNewId().ToString();
            variant.CreatedAt = DateTime.UtcNow;
            variant.ModifiedAt = DateTime.UtcNow;

            product.Variants.Add(variant);

            if (product.AttributeValues == null)
                product.AttributeValues = new Dictionary<string, HashSet<string>>();

            UpdateProductVariantAttributeValues(product);

            var filter = Builders<Product>.Filter.Eq(p => p.Id, productId);
            await _productCollection.ReplaceOneAsync(filter, product);
        }

        public async Task UpdateVariantAsync(string productId, string variantId, ProductVariant updatedVariant)
        {
            var product = await _productCollection.Find(p => p.Id == productId).FirstOrDefaultAsync();
            if (product == null)
                throw new Exception("Product not found");

            var index = product.Variants?.FindIndex(v => v.Id == variantId);
            if (index == -1)
                throw new Exception("Variant not found");

            if (product.Variants == null)
            {
                product.Variants = new();
            }
            // Replace the variant
            updatedVariant.ModifiedAt = DateTime.UtcNow;
            updatedVariant.CreatedAt = product.Variants[index ?? 0].CreatedAt;
            product.Variants[index??0] = updatedVariant;

            UpdateProductVariantAttributeValues(product);

            var filter = Builders<Product>.Filter.Eq(p => p.Id, productId);
            await _productCollection.ReplaceOneAsync(filter, product);
        }

        public async Task DeleteVariantAsync(string productId, string variantId)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, productId);
            var update = Builders<Product>.Update.PullFilter(p => p.Variants, v => v.Id == variantId);
            await _productCollection.UpdateOneAsync(filter, update);
        }

        public async Task<ProductVariant?> GetVariantByIdAsync(string productId, string variantId)
        {
            var product = await GetByIdAsync(productId);
            return product?.Variants?.FirstOrDefault(v => v.Id == variantId);
        }

        public async Task<List<ProductVariant>> GetVariantsAsync(string productId)
        {
            var product = await GetByIdAsync(productId);
            return product?.Variants ?? new List<ProductVariant>();
        }

    }
}
