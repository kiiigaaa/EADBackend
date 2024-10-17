// File Name: CartItem.cs
// Date : 08/10/2024
// This class represents a cart item in the e-commerce application. It contains properties for the item's identifier, customer details, product information, quantity, creation/modification metadata, and active status.

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WebApi.Models
{
    public class CartItem
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
    }
}
