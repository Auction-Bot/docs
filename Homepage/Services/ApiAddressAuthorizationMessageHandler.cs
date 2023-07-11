using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components;

namespace Homepage.Services
{
    public class ApiAddressAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public ApiAddressAuthorizationMessageHandler(IConfiguration configuration, IAccessTokenProvider provider, NavigationManager navigationManager)
            : base(provider, navigationManager)
        {
            var apiUri = new Uri(configuration["API:BaseUri"]!);
            ConfigureHandler(new[] { navigationManager.BaseUri, apiUri.AbsoluteUri });
        }
    }
}
