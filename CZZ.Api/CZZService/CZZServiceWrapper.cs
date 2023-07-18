namespace CZZ.Api.CZZService;

public class CZZServiceWrapper : ICZZServiceWrapper
{
    public IHouseObjectService _houseObjectService;

    public CZZServiceWrapper(IHouseObjectService houseObjectService)
    {
        _houseObjectService = houseObjectService;
    }

    public IHouseObjectService HouseObjectService => _houseObjectService;
}
