using System.ComponentModel.DataAnnotations;

namespace API_Techmove.Models
{
    public class Contract
    {
        [Required] public int ContractId { get; set; }
        [Required] public DateOnly StartDate { get; set; }
        [Required] public DateOnly EndDate { get; set; }

        public string? Status { get; set; }

        [Required] public string ServiceLevel { get; set; } = string.Empty;
        public string? FilePath { get; set; }

        [Required]
        public int ClientId { get; set; }
        public Client? Client { get; set; }


    }
}
