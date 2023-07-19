namespace CZZ.Crawler;

internal class Appsetting
{
    internal static string JsonName;
    internal static string JsonPath;
    internal static bool SearchAll;
    internal static string WebUrl;
    internal static List<FilterData> FilterData;
}
public class AppsettingEntity
{
    public string JsonName = "歷史資料庫名稱.json";
    public string JsonPath = "檔案絕對路徑，若與程式同路徑可留空";
    public bool SearchAll = false;
    public string WebUrl = "網站首頁";
    public List<FilterData> FilterData = new();
}
public class FilterData
{
    public string Url { get; set; }
    public string Desc { get; set; }
}
