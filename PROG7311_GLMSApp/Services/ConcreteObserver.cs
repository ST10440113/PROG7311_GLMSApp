using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.Identity.Client;

namespace PROG7311_GLMSApp.Services
{
    public class Notification : IServiceRequestObserver
    {
       
        private string _client;
        private int _contractId;
        public Notification(int contractId, string client) 
        {
           _contractId = contractId;
           _client = client;
        } 
        
        public void Update(string message)
        {
           message = $"Contract {_contractId} for {_client} has expired.";
        }
    }
}
 