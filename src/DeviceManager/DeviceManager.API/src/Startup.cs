/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by: Atilla Tanrikulu
 * @Last Modified time: 2018-05-18 17:18:18
 */
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.IO.Compression;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using Core.Framework.Service;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Swashbuckle.AspNetCore.Swagger;

namespace DeviceManager.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;
            //Starting Service...        
            string configFilePath = Configuration.GetSection("App").GetSection("ConfigFilePath").Value;
            DeviceManager.Service.Startup.Start($"{configFilePath}/DeviceManager.{HostingEnvironment.EnvironmentName}.config");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddMvc()

            .AddJsonOptions(options =>
             {
                 options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                 options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
                 options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
             });

            services.AddAuthorization()
            .AddAuthentication("Bearer")
            .AddIdentityServerAuthentication(options =>
            {                
                options.Authority = "http://localhost:5000";
                options.RequireHttpsMetadata = false;
                options.ApiName = "devicemanager.api";
            });

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
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
                app.UseExceptionHandler("/error");
            }

            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var error = context.Features.Get<IExceptionHandlerFeature>();

                    //DeviceManager.Startup.Log.Error(error.Error);
                    Console.WriteLine(error.Error);
                    string language = "tr-TR";
                    if (!string.IsNullOrEmpty(context?.Request?.Headers["Accept-Language"]))
                    {
                        language = context?.Request?.Headers["Accept-Language"];
                    }
                    Console.Write(error.Error.Message);
                    Console.Write(error.Error.StackTrace);

                    if (error != null)
                    {
                        LanguageService ls = Core.Framework.Application.Current.GetService<LanguageService>();
                        Exception ex = error.Error;

                        int status = 500;
                        String message = String.Empty;

                        var exceptionType = ex.GetType();
                        if (exceptionType == typeof(UnauthorizedAccessException))
                        {
                            message = "Unauthorized Access";
                            status = (int)HttpStatusCode.Unauthorized;
                        }
                        else if (exceptionType == typeof(NotImplementedException))
                        {
                            message = "A server error occurred.";
                            status = (int)HttpStatusCode.NotImplemented;
                        }
                        else if (exceptionType == typeof(ServiceException))
                        {
                            ServiceException sx = ex as ServiceException;
                            message = sx.Message + ex.StackTrace;
                            status = 500;//(int)sx.ExceptionType;
                        }
                        else if (ex.Message.StartsWith(MessagesConstants.ERR_ORA_02291))
                        {
                            message = ls.Translate(MessagesConstants.ERR_ORA_02291, language);
                            status = (int)HttpStatusCode.InternalServerError;
                        }
                        else if (ex.Message.StartsWith(MessagesConstants.ERR_ORA_00001))
                        {
                            message = ls.Translate(MessagesConstants.ERR_ORA_00001, language);
                            status = (int)HttpStatusCode.InternalServerError;
                        }
                        else if (ex.Message.StartsWith(MessagesConstants.ERR_ORA_1722))
                        {
                            message = ls.Translate(MessagesConstants.ERR_ORA_1722, language);
                            status = (int)HttpStatusCode.InternalServerError;
                        }
                        else if (ex.Message.StartsWith(MessagesConstants.ERR_ORA_01407))
                        {
                            if (ex.Message.Contains("(") && ex.Message.Contains(")") && ex.Message.Contains("\"") && ex.Message.Contains("."))
                            {
                                string column = ex.Message.Substring(ex.Message.IndexOf("("), ex.Message.IndexOf(")") - ex.Message.IndexOf("("));
                                var words = column.Split('.');
                                if (words.Length == 3) column = words[2];
                                else if (words.Length == 2) column = words[1];
                                else column = words[1];
                                message = ls.Translate("LBL_" + column.Split('\"')[1], language) + ls.Translate(MessagesConstants.ERR_ORA_01407, language);
                            }
                            else
                            {
                                message = ex.Message;
                            }
                            status = (int)HttpStatusCode.InternalServerError;
                        }
                        else if (ex.Message.StartsWith(MessagesConstants.ERR_ORA_12899))
                        {
                            if (ex.Message.Contains("(") && ex.Message.Contains(")") && ex.Message.Contains("\"") && ex.Message.Contains("."))
                            {
                                string column = ex.Message.Substring(ex.Message.IndexOf("\""));
                                var words = column.Split('.');
                                if (words.Length == 3) column = words[2];
                                else if (words.Length == 2) column = words[1];
                                else column = words[1];
                                message = ls.Translate("LBL_" + column.Split('\"')[1], language) + ls.Translate(MessagesConstants.ERR_ORA_12899, language);
                            }
                            else
                            {
                                message = ex.Message;
                            }
                            status = (int)HttpStatusCode.InternalServerError;
                        }
                        else
                        {
                            message = ls.Translate(MessagesConstants.ERR_OCCURRED, language) + ": " + ex.Message + ex.StackTrace;
                            status = (int)HttpStatusCode.InternalServerError;
                        }

                        HttpResponse response = context.Response;
                        IHeaderDictionary headers = response.Headers;
                        if (!headers.ContainsKey("Access-Control-Allow-Credentials")) headers.Add("Access-Control-Allow-Credentials", "true");
                        if (!headers.ContainsKey("Pragma")) headers.Add("Pragma", "no-cache");
                        if (!headers.ContainsKey("Cache-Control")) headers.Add("Cache-Control", "no-cache");
                        if (!headers.ContainsKey("Connection")) headers.Add("Connection", "keep-alive");
                        if (!headers.ContainsKey("Access-Control-Allow-Origin")) headers.Add("Access-Control-Allow-Origin", "*");
                        if (!headers.ContainsKey("Access-Control-Allow-Methods")) headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
                        if (!headers.ContainsKey("Access-Control-Allow-Headers")) headers.Add("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept, Key");
                        if (!headers.ContainsKey("Access-Control-Max-Age")) headers.Add("Access-Control-Max-Age", "-1");
                        if (!headers.ContainsKey("Vary")) headers.Add("Vary", "Origin, Accept-Encoding");

                        response.StatusCode = status;
                        response.ContentType = "application/json";
                        ServiceResponse<string> sr = new ServiceResponse<string>(false, ResultType.Error, message);
                        string contentBody = JsonConvert.SerializeObject(sr);
                        ASCIIEncoding encoding = new ASCIIEncoding();
                        int length = encoding.GetBytes(contentBody).Length;
                        //if (!headers.ContainsKey("Content-Length")) headers.Add("Content-Length", (length + 2).ToString());
                        await response.WriteAsync(contentBody);
                    }
                });
            });


            app.UseCors(policy => policy.AllowAnyOrigin()
                                        .AllowAnyHeader()
                                        .AllowAnyMethod());

            var supportedCultures = new[] { new CultureInfo("tr-TR"), new CultureInfo("en-US") };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("tr-TR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseStaticFiles();

            app.UseDefaultFiles();

            app.UseResponseCompression();

            app.UseAuthentication();

            app.UseCoreService();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix="api";
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "default",
                  template: "{controller}/{action}/{id?}",
                  defaults: new { controller = "Home", action = "Index" });

                routes.MapRoute(
                "DefaultController",
                "{*catchall}",
                new { controller = "Default", action = "Index" }
                );
            });

        }
    }
}
