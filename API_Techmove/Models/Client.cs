using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Techmove.Models
{
    public class Client
    {
        [Required] public int ClientId { get; set; }

        [Required] public string FirstName { get; set; } = string.Empty;
        [Required] public string LastName { get; set; } = string.Empty;

        [Required] public string Email { get; set; } = string.Empty;

        [Required] public string PhoneNumber { get; set; } = string.Empty;

        [Required] public string Region { get; set; } = string.Empty;

        [NotMapped] public string? FullName => $"{FirstName} {LastName}";
    }
}
