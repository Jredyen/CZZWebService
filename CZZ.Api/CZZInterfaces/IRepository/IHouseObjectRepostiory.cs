namespace CZZ.Api.CZZInterfaces.IRepository;

public interface IHouseObjectRepostiory
{
    Task<List<string>> GetFilePathEntityAsync();

    Task<RootObject> GetDateObjectEntityAsync(string Date);
}
