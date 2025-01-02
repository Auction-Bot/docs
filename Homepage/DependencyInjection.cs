using Homepage.Models;
using Homepage.Pages;
using Homepage.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;

namespace Homepage
{
    public static class DependencyInjection
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration, string baseAddress)
        {
            var baseUri = new Uri(baseAddress);
            services.AddHttpClient(nameof(FileService), client => client.BaseAddress = baseUri);
            services.AddHttpClient<DiscordAuthenticationStateProvider>(client => client.BaseAddress = baseUri);
            services.AddHttpClient<AgoraApiService>(client => client.BaseAddress = new Uri(configuration["API:BaseUri"]!));
            services.AddHttpClient<AgoraSecuredApiService>(client => client.BaseAddress = new Uri(configuration["API:BaseUri"]!))
                    .AddHttpMessageHandler<DiscordTokenHandler>();

            services.Configure<GitHubSettings>(configuration.GetSection("GitHubSettings"));
            services.AddHttpClient<IGitHubService, GitHubService>((provider, client) =>
            {
                var settings = provider.GetRequiredService<IOptions<GitHubSettings>>().Value;
                client.BaseAddress = new Uri(settings.ApiBaseUrl);
            });

            services.AddScoped<IFileService, FileService>();
            services.AddSingleton<IMenuService, MenuService>();
            services.AddSingleton<IGitHubService, GitHubService>();
            services.AddSingleton<ICommandNavigationService, CommandNavigationService>();
            services.AddScoped<AuthenticationStateProvider, DiscordAuthenticationStateProvider>();
            services.AddSingleton<IRenderQueueService>(new RenderQueueService { Capacity = int.MaxValue });

            services.AddTransient<DiscordTokenHandler>();
            services.AddAuthorizationCore();
        }
    }
}
