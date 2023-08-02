namespace CZZ.Api.CZZInterfaces;

public interface ICZZRepostioryWrapper
{
    public IHouseObjectRepostiory HouseObject { get; }

    public INASRepostiory NASRepostiory { get; }
}
