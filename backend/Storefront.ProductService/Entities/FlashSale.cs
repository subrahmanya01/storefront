using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Storefront.ProductService.Entities
{
    public class FlashSale : BaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = null!;

        public string? ProductId { get; set; } = null!;

        public DateTime StartsAt { get; set; }

        public DateTime EndsAt { get; set;}
    }
}
