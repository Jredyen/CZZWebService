using CZZ.Domain;

namespace CZZ.Api.CZZRepostiory;

public class NASRepostiory : INASRepostiory
{
    public async Task<List<string>> GetNASFilesEntityAsync(string? Folder)
    {
        List<string> result = new();

        string currentDirectory = Directory.GetCurrentDirectory();

        string parentDirectory = Directory.GetParent(currentDirectory).FullName;

        //取得檔案名稱
        try
        {
            FileInfo[] fileInfo = new DirectoryInfo(@$"{parentDirectory}\NAS\{Folder}").GetFiles();
            foreach (var file in fileInfo)
            {
                result.Add(Path.GetFileNameWithoutExtension(file.FullName) + Path.GetExtension(file.FullName));
            }
        }
        catch (DirectoryNotFoundException)
        {
            result.Add("沒有找到資料夾");
        }

        return result;
    }

    public async Task<FolderPathEntity> GetNASFolderEntityAsync(string? Folder)
    {
        FolderPathEntity result = new()
        {
            Folders = new()
        };

        string currentDirectory = Directory.GetCurrentDirectory();

        string parentDirectory = Directory.GetParent(currentDirectory).FullName;

        //取得資料夾名稱
        try
        {
            result.FolderPaht = @$"{Folder}\";

            string[] dirs = Directory.GetDirectories(@$"{parentDirectory}\NAS\{Folder}");
            foreach (string dir in dirs)
            {
                result.Folders.Add(Path.GetFileName(dir));
            }
        }
        catch (DirectoryNotFoundException)
        {
            result.Folders.Add("沒有找到資料夾");
        }

        return result;
    }
}
