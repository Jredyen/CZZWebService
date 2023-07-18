using CZZ.Api.CZZInterfaces;
namespace CZZ.Api.CZZRepostiory
{
    public class CZZRepostioryWrapper : ICZZRepostioryWrapper
    {
        public CZZRepostioryWrapper(IHouseObjectRepostiory houseObject)
        {
            HouseObject = houseObject;
        }

        public IHouseObjectRepostiory HouseObject { get; }
    }
}
