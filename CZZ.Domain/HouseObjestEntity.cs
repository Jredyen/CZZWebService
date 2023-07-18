namespace CZZ.Domain;

public class RootObject
{
    public DataObject? Data { get; set; }
}

public class DataObject
{
    public List<DataItem>? Data { get; set; }
}

public class DataItem
{
    /// <summary>
    /// PK 物件Id
    /// </summary>
    public int? Post_id { get; set; }
    /// <summary>
    /// 更新時間
    /// </summary>
    public string? Refresh_time { get; set; }
    /// <summary>
    /// 物件標題
    /// </summary>
    public string? Title { get; set; }
    /// <summary>
    /// 房屋類型
    /// </summary>
    public string? Kind_name { get; set; }
    /// <summary>
    /// 廳數 (只有整層住家有)
    /// </summary>
    public string? Room_str { get; set; }
    /// <summary>
    /// 樓層
    /// </summary>
    public string? Floor_str { get; set; }
    /// <summary>
    /// 租金
    /// </summary>
    public string? Price { get; set; }
    /// <summary>
    /// 租金單位與時間 (元/月)
    /// </summary>
    public string? Price_unit { get; set; }
    /// <summary>
    /// 物件圖片
    /// </summary>
    public List<string>? Photo_list { get; set; }
    /// <summary>
    /// 行政區
    /// </summary>
    public string? Section_name { get; set; }
    /// <summary>
    /// 街道名稱
    /// </summary>
    public string? Street_name { get; set; }
    /// <summary>
    /// 具體位置
    /// </summary>
    public string? Location { get; set; }
    /// <summary>
    /// 物件標籤
    /// </summary>
    public List<RentTagItem>? Rent_tag { get; set; }
    /// <summary>
    /// 物件坪數
    /// </summary>
    public string? Area { get; set; }
    /// <summary>
    /// 賣家身分
    /// </summary>
    public string? Role_name { get; set; }
    /// <summary>
    /// 賣家稱謂
    /// </summary>
    public string? Contact { get; set; }
    /// <summary>
    /// 最近的大眾運輸站
    /// </summary>
    public Surrounding? Surrounding { get; set; }
}

/// <summary>
/// 物件標籤
/// </summary>
public class RentTagItem
{
    /// <summary>
    /// 標籤名稱
    /// </summary>
    public string? Name { get; set; }
}

/// <summary>
/// 最近的大眾運輸站
/// </summary>
public class Surrounding
{
    /// <summary>
    /// 交通類型
    /// </summary>
    public string? Type { get; set; }
    /// <summary>
    /// 最近的站名
    /// </summary>
    public string? Desc { get; set; }
    /// <summary>
    /// 距離
    /// </summary>
    public string? Distance { get; set; }
}