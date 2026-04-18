using System;

namespace PROG7311_GLMSApp.Services
{
    public class Notifier : ISubject
    {
       private List<IServiceRequestObserver> _observers  = new();  

        public void Subscribe(IServiceRequestObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IServiceRequestObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }
    }


}
