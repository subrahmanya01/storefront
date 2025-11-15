using MongoDB.Bson.Serialization.Attributes;

namespace Storefront.ProductService.Entities
{
    public class BaseEntity
    {
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? CreatedAt { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? ModifiedAt { get; set; }
    }
}
