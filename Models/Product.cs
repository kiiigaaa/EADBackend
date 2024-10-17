// File Name: Product.cs
// Date : 08/10/2024
// This class represents a product in the WebApi application. It defines the properties of a product, including its identification, description, pricing, quantity, and relationships with categories.

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public string? Name { get; set; }

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public int? Quantity { get; set; }

        public List<string> ImageUrls { get; set; } = [];

        public Guid? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool? IsActive { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? CategoryId { get; set; }

        [BsonIgnoreIfNull]
        public string? CategoryName { get; set; }
    }
}
