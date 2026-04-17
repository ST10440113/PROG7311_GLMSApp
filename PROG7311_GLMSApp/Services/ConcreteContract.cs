using PROG7311_GLMSApp.Data;

namespace PROG7311_GLMSApp.Services
{
    public class ConcreteContract
    {
        public class DraftState : IContractStatus
        {
            public bool CreateServiceRequest()
            {
                return false;  
            }
        }

        public class ActiveState : IContractStatus
        {
            public bool CreateServiceRequest()
            {
                return true;
            }

        }

        public class OnHoldState : IContractStatus
        {
            public bool CreateServiceRequest()
            {
                return false;
            }
        }

        public class ExpiredState : IContractStatus
        {
            public bool CreateServiceRequest()
            {
                return false;
            }
        }

    }
}
