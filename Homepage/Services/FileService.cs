namespace Homepage.Services;

public interface IFileService
{
    Task<string?> GetFileContentAsync(string fileName);
    bool IsFileLoaded(string fileName);
}

public class FileService(IHttpClientFactory httpClientFactory) : IFileService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly Dictionary<string, string> _loadedFiles = [];

    public bool IsFileLoaded(string fileName) => _loadedFiles.ContainsKey(fileName);

    public async Task<string?> GetFileContentAsync(string fileName)
    {
        if (_loadedFiles.TryGetValue(fileName, out string? cachedContent))
            return cachedContent;

        var httpClient = _httpClientFactory.CreateClient(nameof(FileService));

        try
        {
            var content = await httpClient.GetStringAsync($"files/{fileName}");
            _loadedFiles[fileName] = content;

            return content;
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }
}
