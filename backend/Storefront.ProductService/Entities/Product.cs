using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Storefront.ProductService.Entities
{
    public class Product : BaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> AllowedAttributes { get; set; } = new();

        public Dictionary<string, string> BaseAttributes { get; set; } = new();

        public List<ProductVariant>? Variants { get; set; }

        public Dictionary<string, HashSet<string>> AttributeValues { get; set; } = new();
    }

}
