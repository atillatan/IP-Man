/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 11:54:42 
 * @Last Modified by:   Atilla Tanrikulu 
 * @Last Modified time: 2018-04-16 11:54:42 
 */

using System;
using System.Collections.Concurrent;


namespace Core.Framework.Service
{
    public class LanguageService
    {
        public string DefaultLanguage { get; set; }
        public LanguageService(string defaultLanguage)
        {
            this.DefaultLanguage = defaultLanguage;
        }

        public string Translate(string key, string language)
        {
            if (language == null || language.Length.Equals(string.Empty)) language = DefaultLanguage;
            if (language.Length == 2) language = language.Equals("en") ? $"{language}-US" : $"{language}-{language.ToUpper()}";

            object _val = key;
            Cache(language).TryGetValue(key, out _val);
            if(_val!=null) return _val as string;
            else return key as string;
        }


        private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, object>> CacheTable = new ConcurrentDictionary<string, ConcurrentDictionary<string, object>>();

        public ConcurrentDictionary<string, object> Cache(string key)
        {
            ConcurrentDictionary<string, object> internalCache;
            if (CacheTable.TryGetValue(key, out internalCache)) return internalCache;

            ConcurrentDictionary<string, object> newCache = new ConcurrentDictionary<string, object>();

            CacheTable[key] = newCache;

            return newCache;
        }
    }

    public static class LanguageServiceExtension
    {
        public static Application AddLanguageService<T>(this Application app, params object[] args)
        {
            if (args == null || args.Length == 0)
                app.Services.TryAdd(typeof(T), (T)Activator.CreateInstance(typeof(T)));
            else
                app.Services.TryAdd(typeof(T), (T)Activator.CreateInstance(typeof(T), args));

            return app;
        }
    }
}