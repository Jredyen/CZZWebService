using HtmlAgilityPack;

namespace CZZ.Crawler;

internal class Headers
{
    internal static Header GetHeaders(string? url)
    {
        try
        {
            Header header = new();
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    header.RequestedWith = "XMLHttpRequest";
                    header.Cookie = string.Join("; ", response.Headers.GetValues("Set-Cookie"));

                    string? htmlContent = response.Content.ReadAsStringAsync().Result;

                    // 使用 HtmlAgilityPack 解析 HTML
                    HtmlDocument htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(htmlContent);

                    // 尋找 name 為 "csrf-token" 的 <meta> 標籤
                    HtmlNode csrfMetaTag = htmlDoc.DocumentNode.SelectSingleNode("//meta[@name='csrf-token']");

                    // 獲取 <meta> 標籤的 content 屬性值
                    header.CsrfToken = csrfMetaTag.GetAttributeValue("content", "");
                }
                else
                {
                    Console.WriteLine("Error occurred: " + response.StatusCode);
                }
            }
            return header;
        }
        catch (InvalidOperationException ex)
        {
            Program.ConsoleObject($"================== Header 讀取異常! ==================\r\n{ex.Message}\r\n", Program.ConsoleColor.Red);

            Console.ForegroundColor = ConsoleColor.White;

            return null;
        }
    }
}
internal class Header
{
    internal string? CsrfToken { get; set; }
    internal string? RequestedWith { get; set; }
    internal string? Cookie { get; set; }
}
