using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROG7311_GLMSApp.Models
{
    public class Contract
    {
      [Key] public int ContractId { get; set; }
        [Display(Name ="Start Date")]public DateOnly StartDate { get; set; }
       [Display(Name ="End Date")] public DateOnly EndDate { get; set; }

      public string? Status { get; set; }

        [Display(Name = "Service Level")] public string ServiceLevel { get; set; }

      public string? FilePath { get; set; }

        [ForeignKey("ClientId")] 
         public int ClientId { get; set; }
        public Client? Client { get; set; }

       
    }
}