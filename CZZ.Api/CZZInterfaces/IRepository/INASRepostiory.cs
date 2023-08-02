namespace CZZ.Api.CZZInterfaces.IRepository;

public interface INASRepostiory
{
    Task<List<object>> GetNASEntityAsync();
}
