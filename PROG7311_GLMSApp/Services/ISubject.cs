namespace PROG7311_GLMSApp.Services
{
    public interface ISubject
    {
        void Subscribe(IServiceRequestObserver observer);
        void Unsubscribe(IServiceRequestObserver observer);
        void Notify();
    }
}
