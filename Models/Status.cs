// File Name: Status.cs
// Date : 08/10/2024
// Represents the status model for the WebApi application. This class contains properties related to the status, including its ID, name, description, creator, modification details, and active status.

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WebApi.Models
{
    public class Status
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public string? Name { get; set; }

        public string? Description { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool? IsActive { get; set; }
    }
}
