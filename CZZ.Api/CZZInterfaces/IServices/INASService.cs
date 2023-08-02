namespace CZZ.Api.CZZInterfaces.IServices;

public interface INASService
{
    /// <summary>
    /// 獲得該資料夾所有檔案
    /// </summary>
    /// <param name="Folder">資料夾名稱</param>
    /// <returns></returns>
    Task<List<string>> GetAllFilesPath(string? Folder);
    /// <summary>
    /// 獲得該資料夾內所有子資料夾
    /// </summary>
    /// <param name="Folder">資料夾名稱</param>
    /// <returns></returns>
    Task<FolderPathEntity> GetAllFolderPath(string? Folder);
}
