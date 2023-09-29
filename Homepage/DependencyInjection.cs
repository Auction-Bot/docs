using Homepage.Services;
using Homepage.Shared.Interfaces;

namespace Homepage
{
    public static class DependencyInjection
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ApiAddressAuthorizationMessageHandler>();
            services.AddHttpClient<IApiService, AgoraApiService>(client => client.BaseAddress = new Uri(configuration["API:BaseUri"]!));
            services.AddHttpClient<ISecuredApiService, FakeApiService>(client => client.BaseAddress = new Uri(configuration["API:BaseUri"]!))
                    .AddHttpMessageHandler<ApiAddressAuthorizationMessageHandler>();

            services.AddSingleton<CurrentUserService>();
            services.AddSingleton<IMenuService, MenuService>();
            services.AddSingleton<ICommandNavigationService, CommandNavigationService>();
            services.AddSingleton<IRenderQueueService>(new RenderQueueService { Capacity = int.MaxValue });
        }
    }
}
