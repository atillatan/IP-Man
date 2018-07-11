/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by:   Atilla Tanrikulu 
 * @Last Modified time: 2018-04-16 10:10:45 
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using DeviceManager;

namespace DeviceManager.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "DeviceManager.API";

            // Build Kestrel server
            IWebHost host = new WebHostBuilder()
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                var env = hostingContext.HostingEnvironment;
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                      .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                config.SetBasePath(env.ContentRootPath);
                config.AddEnvironmentVariables();
                config.AddCommandLine(args);
            })
            .UseIISIntegration()
            .UseStartup<Startup>()
            .UseUrls("http://localhost:5001")
            .Build();

            host.Run();
        }


    }
}
