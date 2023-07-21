using CZZ.Domain;
using Newtonsoft.Json;

namespace CZZ.Crawler;

internal class Program
{
    /// <summary>
    /// 讀取 appsetting.json 資料
    /// </summary>
    internal static void Main()
    {
        try
        {
            string jsonString = "";

            StreamReader streamReader = new StreamReader("appsetting.json");
            jsonString = streamReader.ReadToEnd();
            streamReader.Close();

            var readSetting = JsonConvert.DeserializeObject<AppsettingEntity>(jsonString);

            Appsetting.JsonName = readSetting.JsonName;
            Appsetting.JsonPath = readSetting.JsonPath;
            Appsetting.SearchAll = readSetting.SearchAll;
            Appsetting.WebUrl = readSetting.WebUrl;
            Appsetting.FilterData = readSetting.FilterData;

            ConsoleObject($"================== 讀取設定檔 appsetting.json 成功! ==================", ConsoleColor.Green);

            SearchStart();
        }
        catch (FileNotFoundException)
        {
            ConsoleObject($"找不到 appsetting.json 檔案，產生檔案", ConsoleColor.Yellow);

            AppsettingEntity appsettingEntity = new();

            for (int i = 1; i < 4; i++)
            {
                FilterData filterData = new()
                {
                    Url = $"條件 {i} 的網址",
                    Desc = $"條件 {i} 的敘述"
                };
                appsettingEntity.FilterData.Add(filterData);
            }
            string json = JsonConvert.SerializeObject(appsettingEntity, Formatting.Indented);

            File.WriteAllText("appsetting.json", json);

            ConsoleObject($"請完成設定後重新執行程式，案任意鍵結束", ConsoleColor.Yellow);

            Console.ForegroundColor = System.ConsoleColor.White;

            Console.ReadKey();

            return;
        }
    }
    internal static void SearchStart()
    {

        try
        {
            //獲得網站的Header
            Header hader = Headers.GetHeaders(Appsetting.WebUrl);

            foreach (var item in Appsetting.FilterData)
            {
                Browse(item.Url, item.Desc, hader);
            }

            ConsoleObject($"================== 30 分鐘後重新搜尋 ==================\r\n", ConsoleColor.White);

            //若 Error.ErrorCount 有累計錯誤，將其歸零
            if (System591.ErrorCount > 0)
            {
                System591.ErrorCount = 0;
            }

            Thread.Sleep(TimeSpan.FromMinutes(30));

            SearchStart();
        }
        catch (Exception ex)
        {
            if (System591.ErrorCount > 2)
            {
                ConsoleObject($"================== 爬蟲連續三次異常 ==================\r\n{ex.Message}\r\n", ConsoleColor.Red);

                Thread.Sleep(TimeSpan.FromMinutes(30));

                System591.ErrorCount = 0;

                SearchStart();
            }
            //若 Error.ErrorCount 小於三次，等待 5 分鐘後重新執行，並累計連續錯誤次數 +1
            else
            {
                ConsoleObject($"================== 爬蟲異常，30 分鐘後重新搜尋 ==================\r\n{ex.Message}\r\n", ConsoleColor.Red);

                Thread.Sleep(TimeSpan.FromMinutes(30));

                System591.ErrorCount++;

                SearchStart();
            }
        }

    }

    /// <summary>
    /// 搜尋該網址的物件
    /// </summary>
    /// <param name="url">網址</param>
    /// <param name="desc">這個搜尋的敘述</param>
    /// <param name="header">標頭</param>
    internal static void Browse(string url, string desc, Header header)
    {
        ConsoleObject($"================== 開始進行 {desc} 的條件搜尋 ==================", ConsoleColor.White);
        RootObject? result = new();

        using (HttpClientHandler handler = new HttpClientHandler())
        {

            using (HttpClient client = new HttpClient(handler))
            {
                if (header == null)
                {
                    ConsoleObject($"================== Hander 沒有資料! ==================\r\n", ConsoleColor.Red);

                    return;
                }

                try
                {
                    //注入 Header
                    client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
                    client.DefaultRequestHeaders.Add("X-CSRF-TOKEN", header.CsrfToken);
                    client.DefaultRequestHeaders.Add("Cookie", header.Cookie);

                    string? jsonString = null;

                    //設定 Json 跳過無法對應的物件，避免出錯
                    JsonSerializerSettings settings = new()
                    {
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    jsonString = client.GetStringAsync(url).Result;

                    result = JsonConvert.DeserializeObject<RootObject>(jsonString, settings);

                    if (Appsetting.SearchAll)
                    {
                        for (int count = 30; count <= Convert.ToInt32(result.Records.Replace(",", "")); count += 30)
                        {
                            url += $"&firstRow={count}";

                            jsonString = client.GetStringAsync(url).Result;

                            var resultPage = JsonConvert.DeserializeObject<RootObject>(jsonString, settings);

                            result.Data.Data.AddRange(resultPage.Data.Data);
                        }
                    }

                    //執行比對
                    ObjectCompare(result, desc);
                }
                catch (HttpRequestException ex)
                {
                    ConsoleObject($"================== 網站請求異常 ==================\r\n{ex.Message}\r\n", ConsoleColor.Red);
                    return;
                }
                catch (UriFormatException ex)
                {
                    ConsoleObject($"================== 解析網站時錯誤 ==================\r\n{ex.Message}\r\n", ConsoleColor.Red);
                    return;
                }
                catch (AggregateException ex)
                {
                    ConsoleObject($"================== 解析網站時錯誤 ==================\r\n{ex.Message}\r\n", ConsoleColor.Red);
                    return;
                }
                catch (InvalidOperationException ex)
                {
                    ConsoleObject($"================== 搜尋的網址不正確 ==================\r\n{ex.Message}\r\n", ConsoleColor.Red);
                    return;
                }
            }
        }
    }

    /// <summary>
    /// 比對資料庫有沒有新的物件
    /// </summary>
    /// <param name="searchObject">搜尋到的物件</param>
    /// <param name="desc">這個搜尋的敘述</param>
    internal static void ObjectCompare(RootObject searchObject, string desc)
    {
        string jsonString = "";
        try
        {
            //讀取資料庫檔案
            StreamReader streamReader = new StreamReader(Appsetting.JsonPath + Appsetting.JsonName);
            jsonString = streamReader.ReadToEnd();
            streamReader.Close();

            ConsoleObject("================== 歷史資料庫比對 ==================", ConsoleColor.Yellow);

            JsonSerializerSettings settings = new()
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            SavePostId repositoriesPostId = JsonConvert.DeserializeObject<SavePostId>(jsonString, settings);

            if (repositoriesPostId == null)
            {
                ConsoleObject("================== 資料庫讀取異常，若持續發生請刪除資料庫 ==================\r\n", ConsoleColor.Red);

                return;
            }

            //比對新的物件 Id
            foreach (int Id in searchObject.Data.Data.Select(x => x.Post_id.Value).Except(repositoriesPostId.Post_id.Select(x => x.Value)))
            {
                System591.Post_id.Add(Id);
            }

            //將新的物件 Id 搜尋出來後放入新的容器
            RootObject newObjects = new()
            {
                Data = new()
                {
                    Data = searchObject.Data.Data.Where(x => System591.Post_id.Contains(x.Post_id)).ToList()
                }
            };

            //如果新的容器內有東西，寫入字串
            if (newObjects.Data.Data.Count() > 0)
            {
                //寫入資料庫
                ConsoleObject($"================== 發現 {newObjects.Data.Data.Count()} 筆新物件 ==================\r\n", ConsoleColor.Yellow);

                repositoriesPostId.Post_id.AddRange(newObjects.Data.Data.Select(x => x.Post_id));

                System591.WaitSaveJson = JsonConvert.SerializeObject(repositoriesPostId);

                File.WriteAllText(Appsetting.JsonPath + Appsetting.JsonName, System591.WaitSaveJson, System.Text.Encoding.UTF8);

                SaveTodayObject(newObjects);

                System591.WaitSaveJson = null;

                //將本次搜尋的 Id 清除
                System591.Post_id.Clear();
            }
            else
            {
                ConsoleObject("================== 沒有新物件 ==================\r\n", ConsoleColor.Yellow);

                return;
            }
        }
        catch (FileNotFoundException)
        {
            ConsoleObject("================== 沒有找到資料庫，將這次資料做為資料庫 ==================\r\n", ConsoleColor.Yellow);

            SavePostId save_Post_Id = new();
            save_Post_Id.Post_id.AddRange(searchObject.Data.Data.Select(x => x.Post_id));

            jsonString = JsonConvert.SerializeObject(save_Post_Id);

            File.WriteAllText(Appsetting.JsonPath + Appsetting.JsonName, jsonString, System.Text.Encoding.UTF8);

            return;
        }
    }

    internal static void SaveTodayObject(RootObject searchNewObject)
    {
        string jsonString;
        try
        {
            StreamReader streamReader = new StreamReader(Appsetting.JsonPath + DateTime.Now.ToString("yyyyMMdd") + ".json");
            jsonString = streamReader.ReadToEnd();
            streamReader.Close();

            JsonSerializerSettings settings = new()
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            RootObject repositoriesTodayObject = JsonConvert.DeserializeObject<RootObject>(jsonString, settings);

            repositoriesTodayObject.Data.Data.AddRange(searchNewObject.Data.Data);

            jsonString = JsonConvert.SerializeObject(repositoriesTodayObject);

            File.WriteAllText(Appsetting.JsonPath + DateTime.Now.ToString("yyyyMMdd") + ".json", jsonString, System.Text.Encoding.UTF8);

            ConsoleObject("================== 存入今日資料庫成功! ==================\r\n", ConsoleColor.Green);

            return;
        }
        catch (FileNotFoundException)
        {
            ConsoleObject("================== 還沒有今天的資料庫，開始產生 ==================\r\n", ConsoleColor.Yellow);

            RootObject repositoriesTodayObject = new();
            repositoriesTodayObject.Data = new();
            repositoriesTodayObject.Data.Data = new();

            repositoriesTodayObject.Data.Data.AddRange(searchNewObject.Data.Data);

            jsonString = JsonConvert.SerializeObject(repositoriesTodayObject);

            File.WriteAllText(Appsetting.JsonPath + DateTime.Now.ToString("yyyyMMdd") + ".json", jsonString, System.Text.Encoding.UTF8);

            return;
        }
    }

    /// <summary>
    /// 輸出文字與狀態顏色
    /// </summary>
    /// <param name="Text">輸入的文字</param>
    /// <param name="Color">顏色</param>
    internal static void ConsoleObject(string Text, ConsoleColor Color)
    {
        switch (Color)
        {
            case ConsoleColor.White:
                Console.ForegroundColor = System.ConsoleColor.White;
                Console.WriteLine(DateTime.Now.ToString("T") + " " + Text);
                break;
            case ConsoleColor.Red:
                Console.ForegroundColor = System.ConsoleColor.Red;
                Console.WriteLine(DateTime.Now.ToString("T") + " " + Text);
                break;
            case ConsoleColor.Yellow:
                Console.ForegroundColor = System.ConsoleColor.Yellow;
                Console.WriteLine(DateTime.Now.ToString("T") + " " + Text);
                break;
            case ConsoleColor.Green:
                Console.ForegroundColor = System.ConsoleColor.Green;
                Console.WriteLine(DateTime.Now.ToString("T") + " " + Text);
                break;
        }
    }
    internal enum ConsoleColor
    {
        White,
        Red,
        Yellow,
        Green,
    }
    internal static class System591
    {
        internal static int ErrorCount = 0;

        internal static string? WaitSaveJson = null;

        internal static List<int?> Post_id = new();
    }
}