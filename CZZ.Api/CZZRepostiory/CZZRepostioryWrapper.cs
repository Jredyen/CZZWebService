using CZZ.Api.CZZInterfaces;
namespace CZZ.Api.CZZRepostiory
{
    public class CZZRepostioryWrapper : ICZZRepostioryWrapper
    {
        public CZZRepostioryWrapper(IHouseObjectRepostiory houseObject,
            INASRepostiory nasRepostiory)
        {
            HouseObject = houseObject;

            NASRepostiory = nasRepostiory;
        }

        public IHouseObjectRepostiory HouseObject { get; }

        public INASRepostiory NASRepostiory { get; }
    }
}
