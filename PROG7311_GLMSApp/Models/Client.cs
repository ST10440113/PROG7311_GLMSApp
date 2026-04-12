using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROG7311_GLMSApp.Models
{
    public class Client
    {
        [Key]public int ClientId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        [EmailAddress]public string Email { get; set; }

        [Phone] public string PhoneNumber { get; set; }

        public string Region { get; set; }

        [NotMapped] public string? FullName => $"{FirstName} {LastName}";
    }
}
