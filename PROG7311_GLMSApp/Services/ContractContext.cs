using static PROG7311_GLMSApp.Services.ConcreteContract;

namespace PROG7311_GLMSApp.Services
{
    public class ContractContext
    {
        private IContractStatus _state;
       
        public void SetState(IContractStatus state)
        {
            _state = state;
        }
        
        public bool ChangeState(string status)
        {
            _state = status switch
            {
                "Active" => new ActiveState(),
                "Draft" => new DraftState(),
                "OnHold" => new OnHoldState(),
                "Expired" => new ExpiredState(),
                _ => throw new InvalidOperationException($"Unknown contract status: {status}")
            };

            return _state.CreateServiceRequest(this);
            
        }

    }
    public interface IContractStatus
    {
        bool CreateServiceRequest(ContractContext contractContext);
    }
}
