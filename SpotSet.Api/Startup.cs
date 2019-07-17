using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpotSet.Api.Constants;
using SpotSet.Api.Handlers;

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

            services.RegisterServices();
            services.AddTransient<AppAuthorizationHandler>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddHttpClient(ApiConstants.SetlistClient, client =>
            {
                client.BaseAddress = new Uri(ApiConstants.SetlistUri);
                client.DefaultRequestHeaders.Add(ApiConstants.XApiKey, _setlistApiKey);
                client.DefaultRequestHeaders.Add(ApiConstants.ContentType, ApiConstants.AppJson);
            });
            services.AddHttpClient(ApiConstants.SpotifyClient, client =>
                {
                    client.BaseAddress = new Uri(ApiConstants.SpotifyUri);
                    client.DefaultRequestHeaders.Add(ApiConstants.ContentType, ApiConstants.AppJson);
                })
                .AddHttpMessageHandler<AppAuthorizationHandler>();
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins(ApiConstants.ClientUrlLocal);
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