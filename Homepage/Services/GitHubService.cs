using Homepage.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Options;

namespace Homepage.Services;

public interface IGitHubService
{
    Task CreateBranchAsync(string branchName, string baseBranchName);
    Task UploadFileAsync(string branchName, string filePath, string fileContent);
    Task CreatePullRequestAsync(string branchName, string title, string body, string baseBranchName);
}

public class GitHubService : IGitHubService
{
    private string _token = string.Empty;
    private readonly HttpClient _httpClient;
    private readonly GitHubSettings _settings;
    private readonly AgoraSecuredApiService _agoraApiService;

    public GitHubService(HttpClient httpClient, IOptions<GitHubSettings> settings, AgoraSecuredApiService apiService)
    {
        _agoraApiService = apiService;
        _httpClient = httpClient;
        _settings = settings.Value;

        _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AuctionBot-Docs", "1.0"));
    }

    private async Task SetPersonalAccessTokenAsync()
    {
        if (!string.IsNullOrEmpty(_token)) return;

        _token = await _agoraApiService.GetPersonalAccessToken();
        
        if (string.IsNullOrEmpty(_token)) return;

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", _token);
    }

    public async Task CreateBranchAsync(string branchName, string baseBranchName)
    {
        await SetPersonalAccessTokenAsync();

        var baseSha = await GetBranchShaAsync(baseBranchName);
        var url = $"{_settings.ApiBaseUrl}/repos/{_settings.Owner}/{_settings.Repo}/git/refs";
        var content = new
        {
            @ref = $"refs/heads/{branchName}",
            sha = baseSha
        };
        var response = await _httpClient.PostAsync(url, CreateJsonContent(content));
        response.EnsureSuccessStatusCode();
    }

    public async Task UploadFileAsync(string branchName, string filePath, string fileContent)
    {
        await SetPersonalAccessTokenAsync();

        var url = $"{_settings.ApiBaseUrl}/repos/{_settings.Owner}/{_settings.Repo}/contents/{filePath}";
        var encodedContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(fileContent));
        var uploadContent = new
        {
            message = $"Add {filePath}",
            content = encodedContent,
            branch = branchName
        };
        var response = await _httpClient.PutAsync(url, CreateJsonContent(uploadContent));
        response.EnsureSuccessStatusCode();
    }

    public async Task CreatePullRequestAsync(string branchName, string title, string body, string baseBranchName)
    {
        await SetPersonalAccessTokenAsync();

        var url = $"{_settings.ApiBaseUrl}/repos/{_settings.Owner}/{_settings.Repo}/pulls";
        var prContent = new
        {
            title,
            body,
            head = branchName,
            @base = baseBranchName
        };
        var response = await _httpClient.PostAsync(url, CreateJsonContent(prContent));
        response.EnsureSuccessStatusCode();
    }

    private async Task<string> GetBranchShaAsync(string branchName)
    {
        var url = $"{_settings.ApiBaseUrl}/repos/{_settings.Owner}/{_settings.Repo}/git/ref/heads/{branchName}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        return json.RootElement.GetProperty("object").GetProperty("sha").GetString() ?? string.Empty;
    }

    private static StringContent CreateJsonContent(object obj) => new(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
}

