using PROG7311_GLMSApp.Data;

namespace PROG7311_GLMSApp.Services
{
    public class ConcreteContract 
    {
        public class ActiveState : IContractStatus
        {
            public bool CreateServiceRequest(ContractContext c)
            {
                
                return true;
            }

        }
        public class DraftState : IContractStatus
        {
            public bool CreateServiceRequest(ContractContext c)
            {
                return false;
                
            }
        }

        

        public class OnHoldState : IContractStatus
        {
            public bool CreateServiceRequest(ContractContext c)
            {
                return false;
            }
        }

        public class ExpiredState : IContractStatus
        {
            public bool CreateServiceRequest(ContractContext c)
            {
                return false;
            }
        }

    }
}
