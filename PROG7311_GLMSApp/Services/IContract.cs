namespace PROG7311_GLMSApp.Services
{
    public interface IContract
    {
       string ServiceLevel { get; }
    }

    public class  GoldServiceLevelContract: IContract
    { 
        public string ServiceLevel => "Gold";
    }

    public class SilverServiceLevelContract : IContract
    {
        public string ServiceLevel => "Silver";
    }

    public class BronzeServiceLevelContract : IContract
    {
        public string ServiceLevel => "Bronze";
    }
}
