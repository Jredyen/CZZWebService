namespace CZZ.Api.CZZService;

public class NASService : INASService
{
    private readonly ICZZRepostioryWrapper _czzWrapper;

    public NASService(ICZZRepostioryWrapper czzWrapper)
    {
        _czzWrapper = czzWrapper;
    }

    public async Task<List<string>> GetAllFilesPath(string? Folder)
    {
        var result = await _czzWrapper.NASRepostiory.GetNASFilesEntityAsync(Folder);

        return result;
    }

    public async Task<FolderPathEntity> GetAllFolderPath(string? Folder)
    {
        var result = await _czzWrapper.NASRepostiory.GetNASFolderEntityAsync(Folder);

        return result;
    }
}
