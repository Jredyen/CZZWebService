namespace CZZ.Api.CZZRepostiory;

public class NASRepostiory : INASRepostiory
{
    public async Task<List<object>> GetNASEntityAsync()
    {
        List<object> result = new();
        List<string> folders = new();
        List<string> files = new();

        string currentDirectory = Directory.GetCurrentDirectory();

        string parentDirectory = Directory.GetParent(currentDirectory).FullName;

        //取得資料夾名稱
        string[] dirs = Directory.GetDirectories(parentDirectory + "\\NAS");
        foreach (string dir in dirs)
        {
            folders.Add(Path.GetFileName(dir));
        }

        //取得檔案名稱
        FileInfo[] fileInfo = new DirectoryInfo(parentDirectory + "\\NAS").GetFiles();
        foreach (var file in fileInfo)
        {
            files.Add(Path.GetFileNameWithoutExtension(file.FullName));
        }

        result.Add(folders);
        result.Add(files);

        return result;
    }
}
