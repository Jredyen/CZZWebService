namespace CZZ.Api.CZZService;

public class HouseObjectService : IHouseObjectService
{
    private readonly ICZZRepostioryWrapper _czzWrapper;

    public HouseObjectService(ICZZRepostioryWrapper czzWrapper)
    {
        _czzWrapper = czzWrapper;
    }

    public async Task<List<string>> GetAllFilesPath()
    {
        var result = await _czzWrapper.HouseObject.GetFilePathEntityAsync();

        return result;
    }

    public async Task<RootObject> GetObjectByDate(string Date)
    {
        var result = await _czzWrapper.HouseObject.GetDateObjectEntityAsync(Date);

        return result;
    }
}
