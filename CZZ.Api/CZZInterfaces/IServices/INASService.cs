namespace CZZ.Api.CZZInterfaces.IServices;

public interface INASService
{
    /// <summary>
    /// 獲得資料夾路徑
    /// </summary>
    /// <returns></returns>
    Task<List<object>> GetAllFilesPath();
}
