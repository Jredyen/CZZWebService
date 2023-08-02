namespace CZZ.Api.CZZService;

public class NASService : INASService
{
    private readonly ICZZRepostioryWrapper _czzWrapper;

    public NASService(ICZZRepostioryWrapper czzWrapper)
    {
        _czzWrapper = czzWrapper;
    }

    public async Task<List<object>> GetAllFilesPath()
    {
        var result = await _czzWrapper.NASRepostiory.GetNASEntityAsync();

        return result;
    }
}
