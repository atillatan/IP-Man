/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:08:01 
 * @Last Modified by:   Atilla Tanrikulu 
 * @Last Modified time: 2018-04-16 10:08:01 
 */
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Reflection;
using Core.Framework.Service;

namespace Core.Framework.Middleware
{
    public class AuthorizeMiddleware
    {
        private readonly InvokeDelegate _next;
        private AuthorizationService authorizationService { get; set; }

        public AuthorizationOptions AuthorizationOptions { get; set; }
        public AuthorizeMiddleware(InvokeDelegate next, Action<AuthorizationOptions> setupAction)
        {
            this.AuthorizationOptions = new AuthorizationOptions();
            setupAction.Invoke(this.AuthorizationOptions);
            authorizationService = Application.Current.GetService<AuthorizationService>();
            _next = next;
        }

        public AuthorizeMiddleware(InvokeDelegate next)
        {
            authorizationService = Application.Current.GetService<AuthorizationService>();
            _next = next;
        }

        public async Task Invoke(InvokeContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            ServiceContext serviceContext = context.Request.Properties["ServiceContext"] as ServiceContext;

            MethodCallExpression fbody = context.Request.MethodCallExpression;
            if (fbody == null) throw new ServiceException("Expression must be a method call.");

            MethodInfo methodInfo = (MethodInfo)context.Request.Properties["MethodInfo"];
            object[] customAttributes = (object[])context.Request.Properties["CustomAttributes"];
            if (customAttributes == null) throw new ArgumentNullException("Service Interface method");

            Authorized authorizedAttributes = customAttributes.FirstOrDefault(t => t.GetType() == typeof(Authorized)) as Authorized;

            bool checkAuthorization = false;
            object[] classAttributes = (object[])context.Request.Properties["CustomClassAttributes"];
            Authorized classAttribute = classAttributes.FirstOrDefault(t => t.GetType() == typeof(Authorized)) as Authorized;

            if (classAttribute != null) checkAuthorization = true;
            if (authorizedAttributes == null) checkAuthorization = false;

            if (checkAuthorization && !authorizationService.isAuthorized(authorizedAttributes, serviceContext))
            {
                throw new ServiceException($"Unauthorized access, Method: {context.Request.ServiceInstance.GetType().Name}.{methodInfo.Name}");
            }
            else
            {
                await _next.Invoke(context);
            }

        }
    }

    public static class AuthorizeMiddlewareExtension
    {
        public static IApplicationBuilder UseAttributeAuthorization(this IApplicationBuilder app, Action<AuthorizationOptions> setupAction)
        {
            app.UseMiddleware<AuthorizeMiddleware>(setupAction);
            return app;
        }
        public static IApplicationBuilder UseAttributeAuthorization(this IApplicationBuilder app)
        {
            app.UseMiddleware<AuthorizeMiddleware>();
            return app;
        }
    }


}