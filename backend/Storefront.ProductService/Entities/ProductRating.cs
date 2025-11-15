namespace Storefront.ProductService.Entities
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class ProductRating
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("productId")]
        public string ProductId { get; set; } = null!;

        [BsonElement("userId")]
        public string UserId { get; set; } = null!;

        [BsonElement("stars")]
        public int Stars { get; set; }

        [BsonElement("comment")]
        public string? Comment { get; set; }

        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

}
