using System.ComponentModel.DataAnnotations;

namespace API_Techmove.Models
{
    public class ServiceRequest
    {
        [Required] public int ServiceRequestId { get; set; }

        [Required] public double Cost { get; set; }
        [Required] public string Description { get; set; } = string.Empty;
        [Required] public string Status { get; set; } = string.Empty;

        [Required]
        public int ContractId { get; set; }
        public Contract? Contract { get; set; }
        [Required] public double ZarAmount { get; set; }
    }
}