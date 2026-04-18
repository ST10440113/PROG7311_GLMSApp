using static PROG7311_GLMSApp.Services.ConcreteContract;

namespace PROG7311_GLMSApp.Services
{
    public class ContractContext
    {
        private IContractState _state;
       
        public void SetState(IContractState state)
        {
            _state = state;
        }
        
        public bool ChangeState(string status)
        {
            _state = status switch
            {
                "Active" => new Active(),
                "Draft" => new Draft(),
                "OnHold" => new OnHold(),
                "Expired" => new Expired(),
                _ => throw new InvalidOperationException($"Unknown contract status: {status}")
            };

            return _state.CreateServiceRequest(this);
            
        }

    }
    public interface IContractState
    {
        bool CreateServiceRequest(ContractContext contractContext);
    }
}
