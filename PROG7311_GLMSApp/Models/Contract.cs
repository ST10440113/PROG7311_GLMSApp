using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROG7311_GLMSApp.Models
{
    public class Contract
    {
      [Key] public int ContractId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

      public string Status { get; set; }

       public string ServiceLevel { get; set; }

        [ForeignKey("ClientId")] 
         public int ClientId { get; set; }
        public Client? Client { get; set; }

       
    }
}
