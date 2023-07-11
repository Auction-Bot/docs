using Homepage.Services;

namespace Homepage
{
    public static class DependencyInjection
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<AgoraApiService>(client => client.BaseAddress = new Uri(configuration["API:BaseUri"]!));
            services.AddHttpClient<AgoraSecuredApiService>(client => client.BaseAddress = new Uri(configuration["API:BaseUri"]!))
                            .AddHttpMessageHandler<ApiAddressAuthorizationMessageHandler>();

            services.AddSingleton<IMenuService, MenuService>();
            services.AddSingleton<ICommandNavigationService, CommandNavigationService>();
        }
    }
}
