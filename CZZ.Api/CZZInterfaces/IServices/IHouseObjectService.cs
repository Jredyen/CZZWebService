namespace CZZ.Api.CZZInterfaces.IServices;
public interface IHouseObjectService
{
    /// <summary>
    /// 獲得每日新物件日誌
    /// </summary>
    /// <returns></returns>
    Task<List<string>> GetAllFilesPath();
    /// <summary>
    /// 獲得指定日期的新物件
    /// </summary>
    /// <returns></returns>
    Task<RootObject> GetObjectByDate(string Date);
}
