class Program
{
    public static async Task Main(string[] args)
    {
        // Call the async method within Main using await
        int contentLength = await GetUrlContentLengthAsync();

        Console.WriteLine($"Content length: {contentLength}");
    }

    public static async Task<int> GetUrlContentLengthAsync()
    {
        using var client = new HttpClient();

        Task<string> getStringTask = client.GetStringAsync("https://learn.microsoft.com/dotnet");

        DoIndependentWork();

        string contents = await getStringTask;

        return contents.Length;
    }

    public static void DoIndependentWork()
    {
        Console.WriteLine("Working...");
    }
}