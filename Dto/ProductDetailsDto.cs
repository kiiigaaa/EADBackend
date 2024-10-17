using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WebApi.Dto
{
    public class ProductDetailsDto
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
