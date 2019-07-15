using Microsoft.Extensions.DependencyInjection;
using SpotSet.Api.Services;

namespace SpotSet.Api
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterServices(
            this IServiceCollection services)
        {
            services.AddSingleton<ISpotSetService, SpotSetService>();
            services.AddSingleton<ISetlistFmService, SetlistFmService>();
            services.AddSingleton<ISpotifyService, SpotifyService>();
            services.AddSingleton<ISpotifyAuthService, SpotifyAuthService>();
            return services;
        }
    }
}