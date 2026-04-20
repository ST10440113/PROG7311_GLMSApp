using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.Identity.Client;

namespace PROG7311_GLMSApp.Services
{
    public class Notification : IServiceRequestObserver
    {
        private int _contractId;
        public Notification(int contractId) 
        {
           _contractId = contractId;
          
        } 
        
        public void Update(string message)
        {
           message = $"Contract {_contractId} has expired.";
        }
    }
}
 