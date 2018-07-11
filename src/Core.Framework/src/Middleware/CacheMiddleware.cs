/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:09:39 
 * @Last Modified by:   Atilla Tanrikulu 
 * @Last Modified time: 2018-04-16 10:09:39 
 */

using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Core.Framework.Service;

namespace Core.Framework.Middleware
{
    public class CacheMiddleware
    {
        private readonly InvokeDelegate _next;

        public CacheMiddleware(InvokeDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(InvokeContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            MethodCallExpression fbody = context.Request.MethodCallExpression;
            if (fbody == null) throw new ServiceException("Expression must be a method call.");

            await _next.Invoke(context);
        }
    }

    public static class CacheMiddlewareExtension
    {
        public static IApplicationBuilder UseCache(this IApplicationBuilder app)
        {
            app.UseMiddleware<CacheMiddleware>();
            return app;
        }
    }
}