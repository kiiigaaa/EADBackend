// File Name: Status.cs
// Date : 08/10/2024
// This class represents a user in the WebApi application. It extends the MongoIdentityUser and contains properties for user details, including first name, last name, address, and auditing fields for tracking user creation and modifications.

using System.ComponentModel.DataAnnotations;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace WebApi.Models
{
    [CollectionName("Users")]
    public class User : MongoIdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Address { get; set; } = string.Empty;

        public Guid? CreatedBy { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool? IsActive { get; set; }
    }
}
