/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:09:28 
 * @Last Modified by: Atilla Tanrikulu
 * @Last Modified time: 2018-04-16 14:54:44
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Framework.Middleware;

namespace Core.Framework
{
    public class ApplicationBuilder : IApplicationBuilder
    {
        public readonly IList<Func<InvokeDelegate, InvokeDelegate>> _middlewares = new List<Func<InvokeDelegate, InvokeDelegate>>();

        public IDictionary<string, object> Properties { get; }

        private ApplicationBuilder(ApplicationBuilder builder)
        {
            Properties = new CopyOnWriteDictionary<string, object>(builder.Properties, StringComparer.Ordinal);
            this.UseMiddleware<ContextMiddleware>();
        }

        public ApplicationBuilder()
        {
            Properties = new Dictionary<string, object>();
            this.UseMiddleware<ContextMiddleware>();
        }

        public IApplicationBuilder New()
        {
            var result = new ApplicationBuilder(this);
            this.UseMiddleware<ContextMiddleware>();
            return result;
        }

        public InvokeDelegate Build()
        {
            InvokeDelegate next = context => Task.Run(() => { });
            this.UseMiddleware<InvokeMiddleware>();

            foreach (var current in _middlewares.Reverse())
            {
                next = current(next);
            }
            return next;
        }

        public virtual IApplicationBuilder Use(Func<InvokeDelegate, InvokeDelegate> middleware)
        {
            _middlewares.Add(middleware);
            return this;
        }



        private T GetProperty<T>(string key)
        {
            object value;
            return Properties.TryGetValue(key, out value) ? (T)value : default(T);
        }

        private void SetProperty<T>(string key, T value)
        {
            Properties[key] = value;
        }

        // public IServiceProvider ApplicationServices
        // {
        //     get
        //     {
        //         return GetProperty<IServiceProvider>("application.Services");
        //     }
        //     set
        //     {
        //         SetProperty<IServiceProvider>("application.Services", value);
        //     }
        // }


    }
}