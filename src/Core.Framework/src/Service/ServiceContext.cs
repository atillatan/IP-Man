/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by:   Atilla Tanrikulu 
 * @Last Modified time: 2018-04-16 10:10:45 
 */
using System;
using System.Collections.Generic;
using Core.Framework.Middleware;

namespace Core.Framework.Service
{
    public class ServiceContext : IDisposable
    {
        public string ServerIP { get; set; } = "127.0.0.1";
        public string URL { get; set; } = "Unknown URL";
        public string RequestID { get; set; } = Guid.NewGuid().ToString();
        public UserInfo UserInfo { get; set; } = new UserInfo();

        public ApplicationContext ApplicationContext
        {
            get
            {
                return Application.Current.Context;
            }
        }

        public IDictionary<string, object> Items { get; } = new Dictionary<string, object>();

        public T GetItem<T>(string key)
        {
            object value;
            return Items.TryGetValue(key, out value) ? (T)value : default(T);
        }

        public void AddItem(string key, object val)
        {
            Items[key] = val;
        }

        #region Disposing
        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {

                }
                disposed = true;
            }
        }
        ~ServiceContext()
        {
            Dispose(false);
        }
        #endregion
    }
}
