namespace CZZ.Api.CZZService;

public class CZZServiceWrapper : ICZZServiceWrapper
{
    public IHouseObjectService _houseObjectService;

    public INASService _nasService;

    public CZZServiceWrapper(IHouseObjectService houseObjectService,
        INASService nasService)
    {
        _houseObjectService = houseObjectService;

        _nasService = nasService;
    }

    public IHouseObjectService HouseObjectService => _houseObjectService;

    public INASService NASService => _nasService;
}
