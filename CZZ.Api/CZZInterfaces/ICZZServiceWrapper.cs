namespace CZZ.Api.CZZInterfaces;

public interface ICZZServiceWrapper
{
    public IHouseObjectService HouseObjectService { get; }

    public INASService NASService { get; }
}
