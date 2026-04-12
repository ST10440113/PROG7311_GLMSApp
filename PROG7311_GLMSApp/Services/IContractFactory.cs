namespace PROG7311_GLMSApp.Services
{
    public interface IContractFactory
    {
        IContract Create(string serviceLevel);
    }

    public class ContractFactory : IContractFactory
    {
        public IContract Create(string serviceLevel)
        {
            return serviceLevel.ToLower() switch
            {
                "gold" => new GoldServiceLevelContract(),
                "silver" => new SilverServiceLevelContract(),
                "bronze" => new BronzeServiceLevelContract(),
                _ => throw new ArgumentException("Invalid service level chosen")
            };

        }
    }
}
