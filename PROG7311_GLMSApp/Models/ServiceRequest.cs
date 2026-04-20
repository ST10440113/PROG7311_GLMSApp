using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROG7311_GLMSApp.Models
{
    public class ServiceRequest
    {
        [Key]public int ServiceRequestId { get; set; }
       
        public double Cost { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        
        [ForeignKey("ContractId")] 
        public int ContractId { get; set; }
        public Contract? Contract { get; set; }
        public double ZarAmount { get;  set; }
    }
}
