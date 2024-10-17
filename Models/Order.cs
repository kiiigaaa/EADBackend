// File Name: Order.cs
// Date : 08/10/2024
// This class represents an order in the WebApi application. It contains properties related to the order, such as the customer ID, item count, products, total price, status, and auditing fields for tracking creation and modification details.

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using WebApi.Dto;

namespace WebApi.Models
{
    public class Order
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
    }
}
