// File Name: Role.cs
// Date : 08/10/2024
// This class represents a role in the WebApi application. It inherits from MongoIdentityRole and includes additional properties such as Description, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn, and IsActive to manage role details and track its lifecycle.

using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace WebApi.Models
{
    [CollectionName("Roles")]
    public class Role : MongoIdentityRole
    {
        public string? Description { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool? IsActive { get; set; }
    }
}
