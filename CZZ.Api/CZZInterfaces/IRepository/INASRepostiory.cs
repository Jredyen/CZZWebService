namespace CZZ.Api.CZZInterfaces.IRepository;

public interface INASRepostiory
{
    Task<List<string>> GetNASFilesEntityAsync(string? Folder);
    Task<FolderPathEntity> GetNASFolderEntityAsync(string? Folder);
}
