using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WebApi.Dto
{
    public class RoleDto
    {
        public string RoleName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public Guid? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool? IsActive { get; set; }

    }
}

