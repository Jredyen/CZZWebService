namespace CZZ.Api.CZZRepostiory;

public class HouseObjectRepostiory : IHouseObjectRepostiory
{
    public async Task<List<string>> GetFilePathEntityAsync()
    {
        string currentDirectory = Directory.GetCurrentDirectory();

        string parentDirectory = Directory.GetParent(currentDirectory).FullName;

        FileInfo[] files = new DirectoryInfo(parentDirectory + "\\Files").GetFiles("*.json");

        FilePathEntity result = new();

        foreach (var file in files)
        {
                result.Paths.Add(Path.GetFileNameWithoutExtension(file.Name));
        }

        return result.Paths;
    }
    public async Task<RootObject> GetDateObjectEntityAsync(string Date)
    {
        string currentDirectory = Directory.GetCurrentDirectory();

        string parentDirectory = Directory.GetParent(currentDirectory).FullName;

        string jsonString = "";

        using (var streamReader = new StreamReader(@$"{parentDirectory}\files\{Date}.json"))
        {
            jsonString = await streamReader.ReadToEndAsync();
        }

        JsonSerializerSettings settings = new()
        {
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        RootObject? result = JsonConvert.DeserializeObject<RootObject>(jsonString, settings);

        return result;
    }
}
