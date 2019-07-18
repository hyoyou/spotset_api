using System;
using Microsoft.Extensions.DependencyInjection;
using SpotSet.Api.Constants;
using SpotSet.Api.Handlers;
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
        
        public static IServiceCollection RegisterHttpServices(
            this IServiceCollection services)
        {
            services.AddTransient<AppAuthorizationHandler>();
            services.AddHttpClient(ApiConstants.SpotifyClient, client =>
                {
                    client.BaseAddress = new Uri(ApiConstants.SpotifyUri);
                    client.DefaultRequestHeaders.Add(ApiConstants.ContentType, ApiConstants.AppJson);
                })
                .AddHttpMessageHandler<AppAuthorizationHandler>();
            
            return services;
        }
    }
}