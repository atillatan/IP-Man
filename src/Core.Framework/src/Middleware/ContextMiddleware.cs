/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:08:14 
 * @Last Modified by:   Atilla Tanrikulu 
 * @Last Modified time: 2018-04-16 10:08:14 
 */

using Core.Framework.Service;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Core.Framework.Middleware
{
    public class ContextMiddleware
    {
        private readonly InvokeDelegate _next;
        public ContextMiddleware(InvokeDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(InvokeContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            MethodCallExpression fbody = context.Request.MethodCallExpression;

            //Putting MethodInfo to context
            object methodInfoTemp = null;
            if (!context.Request.Properties.TryGetValue("MethodInfo", out methodInfoTemp))
            {
                methodInfoTemp = fbody.Method;
                context.Request.Properties["MethodInfo"] = (MethodInfo)methodInfoTemp;
            }
            MethodInfo methodInfo = (MethodInfo)methodInfoTemp;

            //Putting ServiceContext to context
            object serviceContextTemp = null;
            if (!context.Request.Properties.TryGetValue("ServiceContext", out serviceContextTemp))
            {
                //TODO : atilla dymaic objeden alinacak
                var serviceContext = context.Request?.ServiceInstance.GetType()?.GetProperty("ServiceContext");

                if (serviceContext != null)
                    context.Request.Properties["ServiceContext"] = (ServiceContext)serviceContext.GetValue(context.Request.ServiceInstance);

            }

            //Putting Interfaces to cache and context
            object _interfaceTemp = null;
            string serviceTypeName = context.Request.ServiceInstance.GetType().Name;
            if (!Cache("Interface").TryGetValue(serviceTypeName, out _interfaceTemp))
            {
                if (context.Request.ServiceInstance.GetType().GetInterfaces().Count() > 1)
                {
                    _interfaceTemp = context.Request.ServiceInstance.GetType().GetInterfaces()[1];
                    Cache("Interface")[serviceTypeName] = _interfaceTemp;
                }
            }
            if (_interfaceTemp != null)
                context.Request.Properties["Interface"] = (Type)_interfaceTemp;

            //Putting Custom attribute to cache and context
            object _customAttributesTemp = null;
            string customAttributeKey = $"{serviceTypeName}.{methodInfo.Name}";
            if (!Cache("CustomAttributes").TryGetValue(customAttributeKey, out _customAttributesTemp))
            {
                if (context.Request.Properties.Keys.Contains("Interface"))
                {
                    Type _interface = (Type)context.Request.Properties["Interface"];
                    Type _int = (Type)_interface;
                    _customAttributesTemp = _int.GetMethod(methodInfo.Name)?.GetCustomAttributes(true);
                }
                else
                {
                    _customAttributesTemp = context.Request.ServiceInstance.GetType().GetMethod(methodInfo.Name)?.GetCustomAttributes(true);
                }

                Cache("CustomAttributes")[customAttributeKey] = _customAttributesTemp;
            }

            if (_customAttributesTemp != null)
                context.Request.Properties["CustomAttributes"] = (object[])_customAttributesTemp;

            //Putting Custom Class Attributes to cache and context
            object _customClassAttributesTemp = null;
            string customClassAttributeKey = $"{serviceTypeName}.Attributes";
            if (!Cache("CustomClassAttributes").TryGetValue(customClassAttributeKey, out _customClassAttributesTemp))
            {
                _customClassAttributesTemp = context.Request.ServiceInstance.GetType().GetCustomAttributes();
                Cache("CustomClassAttributes")[customClassAttributeKey] = _customClassAttributesTemp;
            }
            if (_customClassAttributesTemp != null)
                context.Request.Properties["CustomClassAttributes"] = (object[])_customClassAttributesTemp;


            await _next.Invoke(context);
        }

        private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, object>> CacheTable = new ConcurrentDictionary<string, ConcurrentDictionary<string, object>>();

        private static ConcurrentDictionary<string, object> Cache(string key)
        {
            ConcurrentDictionary<string, object> internalCache;
            if (CacheTable.TryGetValue(key, out internalCache)) return internalCache;

            ConcurrentDictionary<string, object> newCache = new ConcurrentDictionary<string, object>();

            CacheTable[key] = newCache;

            return newCache;
        }

    }

    public static class ContextMiddlewareExtension
    {
        public static IApplicationBuilder UseContext(this IApplicationBuilder app)
        {
            app.UseMiddleware<ContextMiddleware>();
            return app;
        }
    }
}