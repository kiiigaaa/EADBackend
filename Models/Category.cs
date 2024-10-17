// File Name: Category.cs
// Date : 08/10/2024
// Represents a category model for the WebApi application. This class is used to define the structure of a category document stored in MongoDB, including properties for categorization, creation, modification, and status.

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WebApi.Models
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public string? CategoryName { get; set; }

        public string? CategoryDescription { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool? IsActive { get; set; }
    }

}
