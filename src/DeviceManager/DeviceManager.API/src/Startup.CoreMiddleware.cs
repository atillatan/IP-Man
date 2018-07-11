/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by: Atilla Tanrikulu
 * @Last Modified time: 2018-04-16 16:03:24
 */

using Core.Framework.Service;
using Core.Framework.Util;
using log4net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DeviceManager.API
{
    public class CoreMiddleware
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(CoreMiddleware));
        private readonly RequestDelegate _next;
        public CoreMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            IServiceProvider _aspNetServiceProvider = httpContext.RequestServices;
            IConfiguration _config = (IConfiguration)_aspNetServiceProvider.GetService(typeof(IConfiguration));
            IHostingEnvironment _hostingEnvironment = (IHostingEnvironment)_aspNetServiceProvider.GetService(typeof(IHostingEnvironment));

            if (httpContext?.Request?.Path != null)
            {

                ServiceContext serviceContext = new ServiceContext()
                {
                    URL = httpContext.Request.GetDisplayUrl(),
                    ServerIP = httpContext?.Request?.Host.Value,
                    RequestID = httpContext?.TraceIdentifier,
                    UserInfo = new UserInfo() { Claims = httpContext.User.Claims }
                };

                if (serviceContext.UserInfo.Username == null)
                    serviceContext.UserInfo.Claims = new List<Claim>() { new Claim("role", "Anonymous"), new Claim("sub", ConfigManager.Get<string>("user.anonymous", "3")) };

                if (log.IsDebugEnabled) log.Debug($"User: {serviceContext.UserInfo.Username} URL={httpContext?.Request?.Path}");

                string language = httpContext.Request.Headers["Accept-Language"];
                if (!string.IsNullOrEmpty(language))
                    serviceContext.UserInfo.Language = language.Equals("en") ? $"{language}-US" : $"{language}-{language.ToUpper()}";
                else
                    serviceContext.UserInfo.Language = ConfigManager.Get<string>("ui.language.default", "tr-TR");

                serviceContext.Items["Application"] = "DeviceManager.API";
                httpContext.Items["ServiceContext"] = serviceContext;
            }

            await _next.Invoke(httpContext);

            ServiceContext ctx = (ServiceContext)httpContext.Items["ServiceContext"];
            ctx.Dispose();
            httpContext.Items["ServiceContext"] = null;
        }
    }
    public static class MiddlewareExtension
    {
        public static IApplicationBuilder UseCoreService(this IApplicationBuilder app)
        {
            app.UseMiddleware<CoreMiddleware>();
            return app;
        }
    }
}