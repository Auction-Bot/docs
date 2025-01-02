namespace Homepage.Models;

public class GitHubSettings
{
    public string ApiBaseUrl { get; set; } = "https://api.github.com";
    public string Owner { get; set; } = string.Empty;
    public string Repo { get; set; } = string.Empty;
}
