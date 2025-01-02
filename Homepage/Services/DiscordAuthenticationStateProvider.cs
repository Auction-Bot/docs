namespace Homepage.Services;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;

public class DiscordAuthenticationStateProvider(IJSRuntime jsRuntime, HttpClient httpClient) : AuthenticationStateProvider
{
    private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var token = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "discord_token");

            if (string.IsNullOrEmpty(token))
                return new AuthenticationState(_anonymous);

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.GetAsync("https://discord.com/api/users/@me");

            if (!response.IsSuccessStatusCode)
            {
                await jsRuntime.InvokeVoidAsync("localStorage.removeItem", "discord_token");
                return new AuthenticationState(_anonymous);
            }

            var userData = await response.Content.ReadFromJsonAsync<DiscordUser>();
            var identity = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, userData!.Id),
                new Claim(ClaimTypes.Name, userData.Username),
                new Claim(ClaimTypes.Email, userData.Email ?? ""),
            ], "Discord");

            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }
        catch
        {
            return new AuthenticationState(_anonymous);
        }
    }

    public async Task SetTokenAsync(string token)
    {
        await jsRuntime.InvokeVoidAsync("localStorage.setItem", "discord_token", token);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task LogoutAsync()
    {
        await jsRuntime.InvokeVoidAsync("localStorage.removeItem", "discord_token");
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}

public record DiscordUser
{
    public string Id { get; set; } = "";
    public string Username { get; set; } = "";
    public string? Email { get; set; }
    public string? Avatar { get; set; }
}

public record DiscordTokenResponse
{
    public string Access_token { get; set; } = "";
    public string Token_type { get; set; } = "";
    public int Expires_in { get; set; }
    public string Refresh_token { get; set; } = "";
    public string Scope { get; set; } = "";
}
