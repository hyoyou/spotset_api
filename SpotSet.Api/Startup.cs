using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpotSet.Api.Constants;
using SpotSet.Api.Handlers;
using SpotSet.Api.Services;

namespace SpotSet.Api
{
    public class Startup
    {
        private string _setlistApiKey;
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, reloadOnChange: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            _setlistApiKey = Configuration["SetlistApiKey"];
            
            services.AddTransient<AppAuthorizationHandler>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddHttpClient(HttpConstants.SetlistClient, client =>
            {
                client.BaseAddress = new Uri(HttpConstants.SetlistUri);
                client.DefaultRequestHeaders.Add(HttpConstants.XApiKey, _setlistApiKey);
                client.DefaultRequestHeaders.Add(HttpConstants.ContentType, HttpConstants.AppJson);
            });
            services.AddHttpClient(HttpConstants.SpotifyClient, client =>
                {
                    client.BaseAddress = new Uri(HttpConstants.SpotifyUri);
                    client.DefaultRequestHeaders.Add(HttpConstants.ContentType, HttpConstants.AppJson);
                })
                .AddHttpMessageHandler<AppAuthorizationHandler>();
            services.AddSingleton<ISetlistService, SetlistService>();
            services.AddSingleton<ISpotifyAuthService, SpotifyAuthService>();

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins(HttpConstants.ClientUrlLocal);
                    });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(MyAllowSpecificOrigins);
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}