using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace netcore
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services
            .AddCors()
            .AddMvc()
            .AddJsonOptions(options =>
             {
                 options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                 options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
                 options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
             });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseStaticFiles();

            app.UseDefaultFiles(new DefaultFilesOptions { DefaultFileNames = new List<string> { "index.html" } });

            app.UseMvc(routes =>
           {
               routes.MapRoute(
                 name: "default",
                 template: "{controller}/{action}/{id?}");

               // Catch all Route - catches anything not caught be other routes
               routes.MapRoute(
                   name: "catch-all",
                   template: "{*url}",
                   defaults: new { controller = "Default", action = "Index" }
               );
           });

        }
    }
}
