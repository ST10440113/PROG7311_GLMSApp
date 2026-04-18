namespace PROG7311_GLMSApp.Services
{
    public interface IContract
    {
       string ServiceLevel { get; }
    }

    public class  GoldServiceLevel: IContract
    { 
        public string ServiceLevel => "Gold";
    }

    public class SilverServiceLevel : IContract
    {
        public string ServiceLevel => "Silver";
    }

    public class BronzeServiceLevel : IContract
    {
        public string ServiceLevel => "Bronze";
    }
}
