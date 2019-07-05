using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpotSet.Api.Services;

namespace SpotSet.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddHttpClient("GetSetlistClient", client =>
            {
                client.BaseAddress = new Uri("https://api.setlist.fm/rest/1.0/setlist/");
                client.DefaultRequestHeaders.Add("x-api-key", "dijireBcABogLtlblNC9u8lEYo0MDsSu8blF");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            services.AddSingleton<ISetlistService, SetlistService>();
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000");
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