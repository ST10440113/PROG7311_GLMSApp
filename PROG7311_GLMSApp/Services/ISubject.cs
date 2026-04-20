using PROG7311_GLMSApp.Models;

namespace PROG7311_GLMSApp.Services
{
    public interface ISubject
    {
        void Subscribe(IServiceRequestObserver observer);
        void Unsubscribe(IServiceRequestObserver observer);
        string Notify(Contract contract);
    }
}
