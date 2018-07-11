/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by:   Atilla Tanrikulu 
 * @Last Modified time: 2018-04-16 10:10:45 
 */
using System;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.IO;
using System.Xml;

namespace Core.Framework.Util
{
    public class CacheUtil
    {
        private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, object>> CacheTable = new ConcurrentDictionary<string, ConcurrentDictionary<string, object>>();

        public static ConcurrentDictionary<string, object> Cache(string key)
        {
            ConcurrentDictionary<string, object> internalCache;
            if (CacheTable.TryGetValue(key, out internalCache)) return internalCache;

            ConcurrentDictionary<string, object> newCache = new ConcurrentDictionary<string, object>();

            CacheTable[key] = newCache;

            return newCache;
        }
    }
}