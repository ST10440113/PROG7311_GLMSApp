using PROG7311_GLMSApp.Data;

namespace PROG7311_GLMSApp.Services
{
    public class ConcreteContract 
    {
        public class Active : IContractState
        {
            public bool CreateServiceRequest(ContractContext c)
            {
                
                return true;
            }

        }
        public class Draft : IContractState
        {
            public bool CreateServiceRequest(ContractContext c)
            {
                return false;
                
            }
        }

        

        public class OnHold : IContractState
        {
            public bool CreateServiceRequest(ContractContext c)
            {
                return false;
            }
        }

        public class Expired : IContractState
        {
            public bool CreateServiceRequest(ContractContext c)
            {
                return false;
            }
        }

    }
}
