using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace Storefront.ProductService.Entities
{
    public class ProductVariant : BaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = null!;
        public Dictionary<string, string> Attributes { get; set; } = new();
        public double Price { get; set; }
        public int Inventory { get; set; }
        public List<string> Images { get; set; } = new();

        [BsonIgnore] 
        public bool? IsComplete => Images != null && Images.Count > 0;
    }
}
