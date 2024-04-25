using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace books452
{
    public class ApplicationUser : IdentityUser 
    {
        [Required]
        public string Name { get; set; }

        public string? StreetAddress { get; set; }

        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }

        [NotMapped] //not saved in the db
        public string? RoleName { get; set; }
    }
}
