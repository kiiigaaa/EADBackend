using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WebApi.Dto
{
    public class CartItemDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public Guid CustomerId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; } = string.Empty;

        public int? Quantity { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool? IsActive { get; set; }

        [BsonIgnoreIfNull]
        public string? ProductName { get; set; }

        [BsonIgnoreIfNull]
        public decimal? Price { get; set; }

        [BsonIgnoreIfNull]
        public List<string> ImageUrls { get; set; } = [];
    }
}
