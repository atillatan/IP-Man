/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:07:56 
 * @Last Modified by:   Atilla Tanrikulu 
 * @Last Modified time: 2018-04-16 10:07:56 
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Core.Framework
{
    public partial class Application
    {
        #region Singleton Implementation

        private static Application _application = null;
        private static readonly object SyncRoot = new Object();

        private Application()
        {
        }
        public static Application Current
        {
            get
            {
                if (_application == null)
                {
                    lock (SyncRoot)
                    {
                        if (_application == null)
                            _application = new Application();
                    }
                }
                return _application;
            }
        }

        #endregion Singleton Implementation

        public readonly IApplicationBuilder appBuilder = new ApplicationBuilder();

        public InvokeDelegate app;

        public void Build()
        {
            app = appBuilder.Build();
        }
    }

    public partial class Application
    {
        private ConcurrentDictionary<Type, object> _services = new ConcurrentDictionary<Type, object>();

        public ConcurrentDictionary<Type, object> Services
        {
            get { return _services; }
        }

        public T GetService<T>()
        {
            object value;
            return _services.TryGetValue(typeof(T), out value) ? (T)value : default(T);
        }

        #region Default AddServices
        public Application AddService<T>(params object[] args)
        {
            if (args == null || args.Length == 0)
                _services.TryAdd(typeof(T), (T)Activator.CreateInstance(typeof(T)));
            else
                _services.TryAdd(typeof(T), (T)Activator.CreateInstance(typeof(T), args));

            return this;
        }

        public Application AddService<T>(Action<dynamic> setupAction)
        {
            var svc = (T)Activator.CreateInstance(typeof(T), setupAction);
            _services.TryAdd(typeof(T), svc);
            return this;
        }
        #endregion


        private ApplicationContext _ApplicationContext;

        public ApplicationContext Context
        {
            get
            {
                if (this._ApplicationContext == null)
                    _ApplicationContext = new ApplicationContext();
                return _ApplicationContext;
            }
        }
    }
}