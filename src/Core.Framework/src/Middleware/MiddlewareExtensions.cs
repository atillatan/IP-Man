/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:20 
 * @Last Modified by:   Atilla Tanrikulu 
 * @Last Modified time: 2018-04-16 10:10:20 
 */

using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Core.Framework.Middleware
{
    /// <summary>
    /// Extension methods for adding typed middlware.
    /// </summary>
    public static class MiddlewareExtensions
    {
        private const string InvokeMethodName = "Invoke";

        /// <summary>
        /// Adds a middleware type to the application's request pipeline.
        /// </summary>
        /// <typeparam name="TMiddleware">The middleware type.</typeparam>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance.</param>
        /// <param name="args">The arguments to pass to the middleware type instance's constructor.</param>
        /// <returns>The <see cref="IApplicationBuilder"/> instance.</returns>
        public static IApplicationBuilder UseMiddleware<TMiddleware>(this IApplicationBuilder app, params object[] args)
        {
            return app.UseMiddleware(typeof(TMiddleware), args);
        }

        /// <summary>
        /// Adds a middleware type to the application's request pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance.</param>
        /// <param name="middleware">The middleware type.</param>
        /// <param name="args">The arguments to pass to the middleware type instance's constructor.</param>
        /// <returns>The <see cref="IApplicationBuilder"/> instance.</returns>
        public static IApplicationBuilder UseMiddleware(this IApplicationBuilder app, Type middleware, params object[] args)
        {
            return app.Use(next =>
            {
                var methods = middleware.GetMethods(BindingFlags.Instance | BindingFlags.Public);
                var invokeMethods = methods.Where(m => string.Equals(m.Name, InvokeMethodName, StringComparison.Ordinal)).ToArray();

                if (invokeMethods.Length > 1)
                {
                    throw new InvalidOperationException("Multiple Invoke method exist!");
                }

                if (invokeMethods.Length == 0)
                {
                    throw new InvalidOperationException("There is no Invoke method");
                }

                var methodinfo = invokeMethods[0];
                if (!typeof(Task).IsAssignableFrom(methodinfo.ReturnType))
                {
                    throw new InvalidOperationException("Every middleware must return Task");
                }

                var parameters = methodinfo.GetParameters();
                if (parameters.Length == 0 || parameters[0].ParameterType != typeof(InvokeContext))
                {
                    throw new InvalidOperationException("first parameter must InvokeContext");
                }

                var ctorArgs = new object[args.Length + 1];
                ctorArgs[0] = next;
                Array.Copy(args, 0, ctorArgs, 1, args.Length);
                var instance = Activator.CreateInstance(middleware, ctorArgs);

                if (parameters.Length == 1)
                {
                    return (InvokeDelegate)methodinfo.CreateDelegate(typeof(InvokeDelegate), instance);
                }
                else
                {
                    return null;
                }
            });
        }
    }
}