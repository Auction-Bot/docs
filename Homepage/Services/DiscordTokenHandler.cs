using System.Net.Http.Headers;

namespace Homepage.Services;

internal sealed class DiscordTokenHandler(DiscordAuthenticationStateProvider authStateProvider) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var authState = await authStateProvider.GetAuthenticationStateAsync();

        if (authState.User.Identity?.IsAuthenticated == true)
        {
            var accessToken = authState.User.Claims.Single(c => c.Type == "access_token").Value;

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
