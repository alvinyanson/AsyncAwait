


using System.Diagnostics;
using System.Text.RegularExpressions;


class WordCount
{
    public int Count { get; set; }
    public string Url { get; set; }
}

class Program
{

    private static readonly IEnumerable<string> s_urlList =
        [
            "https://learn.microsoft.com",
            "https://learn.microsoft.com/aspnet/core",
            "https://learn.microsoft.com/azure",
            "https://learn.microsoft.com/azure/devops",
            "https://learn.microsoft.com/dotnet",
            "https://learn.microsoft.com/dotnet/desktop/wpf/get-started/create-app-visual-studio",
            "https://learn.microsoft.com/education",
            "https://learn.microsoft.com/shows/net-core-101/what-is-net",
            "https://learn.microsoft.com/enterprise-mobility-security",
            "https://learn.microsoft.com/gaming",
            "https://learn.microsoft.com/graph",
            "https://learn.microsoft.com/microsoft-365",
            "https://learn.microsoft.com/office",
            "https://learn.microsoft.com/powershell",
            "https://learn.microsoft.com/sql",
            "https://learn.microsoft.com/surface",
            "https://dotnetfoundation.org",
            "https://learn.microsoft.com/visualstudio",
            "https://learn.microsoft.com/windows",
            "https://learn.microsoft.com/maui"
    ];

    private static readonly HttpClient s_httpClient = new();

    public static async Task Main(string[] args)
    {
        Console.WriteLine("Application started...");

        Console.WriteLine("Counting .NET phrase in websites...");

        var timer = new Stopwatch();
        timer.Start();

        await PerformWhenAll();

        //await PerformAsync();

        timer.Stop();

        Console.WriteLine($"Time: {timer.Elapsed.ToString(@"m\:ss\.fff")}");
    }

    public static async Task PerformWhenAll()
    {
        int total = 0;

        List<Task<WordCount>> tasks = [];

        foreach (string url in s_urlList)
        {
            tasks.Add(GetDotNetCount(url));
        }

        var results = await Task.WhenAll(tasks);

        foreach (var item in results)
        {
            Console.WriteLine($"Count: {item.Count} - URL: {item.Url}");
            total += item.Count;
        }

        Console.WriteLine("Total Words Found: " + total);
    }

    public static async Task PerformAsync()
    {
        int total = 0;

        foreach (string url in s_urlList)
        {

            var result = await GetDotNetCount(url);
            Console.WriteLine($"Count: {result.Count} - URL: {result.Url}");
            total += result.Count;
        }

        Console.WriteLine("Total Words Found: " + total);
    }

    public static async Task<WordCount> GetDotNetCount(string Url)
    {
        try
        {
            var html = await s_httpClient.GetStringAsync(Url);
            var count = Regex.Matches(html, @"\.NET").Count;
            return new WordCount { Count = count, Url = Url };
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error in this URL: {Url}");
            Console.WriteLine($"{ex.Message}");
            return new WordCount { Count = 0, Url = Url };
        }

    }
}