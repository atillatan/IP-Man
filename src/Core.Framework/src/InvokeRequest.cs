/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:08 
 * @Last Modified by:   Atilla Tanrikulu 
 * @Last Modified time: 2018-04-16 10:10:08 
 */

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Core.Framework.Service;

namespace Core.Framework
{
    public class InvokeRequest
    {
        public IProxyService Proxy { get; private set; }

        public Expression<Func<object>> Function { get { return Proxy.ExpFunc; } }

        public MethodCallExpression MethodCallExpression { get { return Proxy.MethodCallExpression; } }

        public Object ServiceInstance { get { return Proxy.Instance; } }

        public InvokeRequest(IProxyService proxy)
        {
            this.Proxy = proxy;             
        }

        public UserInfo UserInfo { get; internal set; }

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