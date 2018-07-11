/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:09:55 
 * @Last Modified by:   Atilla Tanrikulu 
 * @Last Modified time: 2018-04-16 10:09:55 
 */

using System.Collections.Generic;
using Core.Framework.Service;

namespace Core.Framework
{
    public class InvokeContext
    {
        public InvokeRequest Request { get; }
        public InvokeResult Result { get; }
        public InvokeContext(IProxyService proxy)
        {
            this.Request = new InvokeRequest(proxy);
            this.Result = new InvokeResult();
        }

        public InvokeContext(IProxyService proxy, object result)
        {
            this.Request = new InvokeRequest(proxy);
            this.Result = new InvokeResult();
        }

        public IDictionary<string, object> Properties { get; } = new Dictionary<string, object>();

        private T GetProperty<T>(string key)
        {
            object value;
            return Properties.TryGetValue(key, out value) ? (T)value : default(T);
        }

        private void SetProperty(string key, object value)
        {
            Properties[key] = value;
        }
    }
}