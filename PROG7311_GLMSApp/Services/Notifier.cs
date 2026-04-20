using PROG7311_GLMSApp.Models;

namespace PROG7311_GLMSApp.Services
{
    public class Notifier: ISubject
    {
        private List<IServiceRequestObserver> _observers = new();

        public void Subscribe(IServiceRequestObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IServiceRequestObserver observer)
        {
            _observers.Remove(observer);
        }

        public string Notify(Contract contract)
        {
            string message = $"Contract {contract.ContractId} has expired.";
            foreach (var observer in _observers)
            {
               observer.Update(message);
            }
            return message;
        }
    }
}   