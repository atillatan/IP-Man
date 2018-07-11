/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by: Atilla Tanrikulu
 * @Last Modified time: 2018-04-16 14:55:44
 */
using System.Collections.Generic;

namespace Core.Framework
{
    public class ApplicationContext
    {
        public ApplicationContext()
        {            
            Items["environment.name"] = "Development";//Default
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
    }
}