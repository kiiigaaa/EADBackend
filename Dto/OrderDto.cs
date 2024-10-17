using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WebApi.Dto
{
    public class OrderDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public Guid CustomerId { get; set; }

        public int? ItemCount { get; set; }

        public List<ProductDetailsDto> Products { get; set; } = [];

        public decimal TotalPrice { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string CurrentStatusId { get; set; } = string.Empty;

        public Guid? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool? IsActive { get; set; }

        public string? Status { get; set; }

        public string? StatusDescription { get; set; }
    }
}
