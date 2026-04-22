using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROG7311_GLMSApp.Models
{
    public class Client
    {
        [Key]public int ClientId { get; set; }

        [Display(Name = "First Name")]public string FirstName { get; set; }
        [Display(Name = "Last Name")]public string LastName { get; set; }
        
       [Display(Name = "Email Address")][EmailAddress]public string Email { get; set; }

        [Display(Name = "Phone Number")][Phone] public string PhoneNumber { get; set; }

        public string Region { get; set; }

        [NotMapped] public string? FullName => $"{FirstName} {LastName}";
    }
}
 