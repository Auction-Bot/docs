using System.Net.Http.Headers;

namespace Homepage.Services;

internal sealed class DiscordTokenHandler(DiscordAuthenticationStateProvider authStateProvider) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var authState = await authStateProvider.GetAuthenticationStateAsync();

        if (authState.User.Identity?.IsAuthenticated == true)
            return await base.SendAsync(request, cancellationToken);

        return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
    }
}
