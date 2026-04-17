using static PROG7311_GLMSApp.Services.ConcreteContract;

namespace PROG7311_GLMSApp.Services
{
    public class ContractContext
    {
        private IContractStatus _state;
        public ContractContext()
        {
            _state = new DraftState();
        }
        public void SetState(IContractStatus state)
        {
            _state = state;
        }
        public bool ChangeState(string status)
        {
          return _state.CreateServiceRequest();
            
        }

    }
    public interface IContractStatus
    {
        bool CreateServiceRequest();
    }
}
