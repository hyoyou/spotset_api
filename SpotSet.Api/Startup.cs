﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpotSet.Api.Constants;

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
            _setlistApiKey = Configuration[ApiConstants.SetlistApiKey];

            services.RegisterServices();
            services.RegisterHttpServices();
            services.AddHttpClient(ApiConstants.SetlistClient, client =>
            {
                client.BaseAddress = new Uri(ApiConstants.SetlistUri);
                client.DefaultRequestHeaders.Add(ApiConstants.XApiKey, _setlistApiKey);
                client.DefaultRequestHeaders.Add(ApiConstants.ContentType, ApiConstants.AppJson);
            });
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins(ApiConstants.ClientUrlLocal);
                    });
            });
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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