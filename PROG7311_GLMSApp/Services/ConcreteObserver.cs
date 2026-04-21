using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.Identity.Client;
using PROG7311_GLMSApp.Models;
using System.Diagnostics.Contracts;

namespace PROG7311_GLMSApp.Services
{
    public class Notification : IServiceRequestObserver
    {
        private int _contractId;
        private string _status;
        public Notification(int contractId, string status) 
        {
           _contractId = contractId;
           _status = status;
        } 
        
        public string Update(string message)
        {
           message = $"Service Request status for Contract {_contractId} is now {_status}";
           return message;
        }
    }
}
