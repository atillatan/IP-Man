/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:17 
 * @Last Modified by: Atilla Tanrikulu
 * @Last Modified time: 2018-04-16 16:00:22
 */

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using Core.Framework.Service;
using log4net;
using log4net.Repository.Hierarchy;
using log4net.Layout;
using log4net.Appender;
using log4net.Core;
using System.IO;
using log4net.Repository;
using System.Collections;
using log4net.Config;

namespace Core.Framework.Middleware
{
    public class LogMiddleware
    {
        private static ILog log = LogManager.GetLogger(typeof(LogMiddleware));
        private readonly InvokeDelegate _next;

        public string ConfigFilePath { get; }
        public bool isLog4NetEnabled { get; set; }

        public LogMiddleware(InvokeDelegate next)
        {
            _next = next;
        }

        public LogMiddleware(InvokeDelegate next, string configFilePath)
        {
            _next = next;
            this.ConfigFilePath = configFilePath;
            Configure(configFilePath);
        }
        public async Task Invoke(InvokeContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            ServiceContext serviceContext = context.Request.Properties["ServiceContext"] as ServiceContext;

            MethodCallExpression fbody = context.Request.MethodCallExpression;
            if (fbody == null) throw new ServiceException("Expression must be a method call.");

            MethodInfo methodInfo = (MethodInfo)context.Request.Properties["MethodInfo"];

            var timer = Stopwatch.StartNew();
            try
            {
                await _next.Invoke(context);
            }
            catch (ServiceException sx)
            {
                log.Debug($"{methodInfo.Name}(): {sx.Message}");
                throw sx;
            }
            catch (Exception ex)
            {
                /* ExceptionMiddleware de yapilacak          
                if (ex.InnerException != null)
                {
                    if (!ex.InnerException.Message.StartsWith(MessagesConstants.ERR_ORA_00001) &&
                        !ex.InnerException.Message.StartsWith(MessagesConstants.ERR_ORA_01407) &&
                        !ex.InnerException.Message.StartsWith(MessagesConstants.ERR_ORA_12899) &&
                        !ex.InnerException.Message.StartsWith(MessagesConstants.ERR_ORA_1722)
                        )
                    {

                        Console.WriteLine(ex);
                        if (Application.Current.Context.GetItem<string>("ENVIRONMENT_NAME") != "Development")
                            throw new Exception("Servis erişiminde hata oluştu, lütfen bir süre sonra tekrar deneyiniz.");

                    }
                }
                */
                log.Error(ex);
                throw ex;
            }
            finally
            {
                timer.Stop();
                if (timer.ElapsedMilliseconds > 1000)
                {
                    if (isLog4NetEnabled) log.Debug($"Long method invokation time in { methodInfo.Name} took {timer.ElapsedMilliseconds} ms");
                    else Console.WriteLine($"Long method invokation time in { methodInfo.Name} took {timer.ElapsedMilliseconds} ms");
                }
                else
                {
                    if (isLog4NetEnabled) log.Debug($"{methodInfo.Name}() method invoked in :{Convert.ToDouble(timer.ElapsedMilliseconds)}ms {serviceContext?.UserInfo?.Username} requestId:{serviceContext?.RequestID}");
                    else Console.WriteLine($"{methodInfo.Name}() method invoked in :{Convert.ToDouble(timer.ElapsedMilliseconds)}ms {serviceContext?.UserInfo?.Username} requestId:{serviceContext?.RequestID}");
                }

                logAuditAsync();
            }
        }

        private void Configure(string configFilePath)
        {
            //load logging configuration
            Console.WriteLine($"Loading log4net configuration  path:{configFilePath}");

            //Log4net config yoksa elle configure et
            if (File.Exists(configFilePath))
            {
                FileInfo logConfig = new FileInfo(configFilePath);
                ILoggerRepository repository = LogManager.GetRepository(Assembly.GetCallingAssembly());
                log4net.GlobalContext.Properties["LOG_HOME"] = Environment.GetEnvironmentVariable("LOG_HOME");
                ICollection col = XmlConfigurator.ConfigureAndWatch(repository, logConfig);
                log = LogManager.GetLogger(typeof(LogMiddleware));
                log.Debug($"Log4Net Configuration loaded:{logConfig.FullName}");
                isLog4NetEnabled = true;
            }
            else
            {
                Console.WriteLine($"log4net configuration not found path:{configFilePath}");
                Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository(Assembly.Load("Core.Framework"));

                PatternLayout patternLayout = new PatternLayout();
                patternLayout.ConversionPattern = "[%date][%-5level][thread:%thread] %message [%logger.%line]%newline";
                patternLayout.ActivateOptions();

                RollingFileAppender roller = new RollingFileAppender();
                roller.AppendToFile = true;
                roller.File = @"Logs/debug.log";
                roller.Layout = patternLayout;
                roller.MaxSizeRollBackups = 10;
                roller.MaximumFileSize = "5000KB";
                roller.RollingStyle = RollingFileAppender.RollingMode.Size;
                roller.StaticLogFileName = true;
                roller.ActivateOptions();
                roller.LockingModel = new FileAppender.MinimalLock();
                hierarchy.Root.AddAppender(roller);

                MemoryAppender memory = new MemoryAppender();
                memory.ActivateOptions();
                hierarchy.Root.AddAppender(memory);

                hierarchy.Root.Level = Level.Debug;
                hierarchy.Configured = true;
                log.Debug($"Log4Net Configuration loaded:dynamically");
            }
        }

        private static void logAuditAsync()
        {
            //TODO:Atilla daha sonra yapilacak
            // if (log.IsDebugEnabled) log.Debug("Writing an Audit log...");
        }
    }

    public static class LogMiddlewareExtension
    {
        public static IApplicationBuilder UseLog(this IApplicationBuilder app)
        {
            app.UseMiddleware<LogMiddleware>();
            return app;
        }

        public static IApplicationBuilder UseLog(this IApplicationBuilder app, string configFilePath)
        {
            app.UseMiddleware<LogMiddleware>(configFilePath);
            return app;
        }
    }
}