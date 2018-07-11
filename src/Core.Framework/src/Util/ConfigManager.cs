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
    /// <summary>
    /// this class will be monitor configFile info
    /// </summary>
    public class ConfigManager
    {
        #region Singleton Implementation

        private static ConfigManager _configManager = null;
        private static readonly object SyncRoot = new Object();

        public static ConfigManager Current
        {
            get
            {
                if (_configManager == null)
                {
                    lock (SyncRoot)
                    {
                        if (_configManager == null)
                            _configManager = new ConfigManager();
                    }
                }
                return _configManager;
            }
        }

        private ConfigManager()
        {
            Initialize();
        }

        #endregion Singleton Implementation

        public static FileInfo ConfigFileInfo { get; }
        private ConcurrentDictionary<string, string> _configurations;
        public ConcurrentDictionary<string, string> Configurations { get { return _configurations; } }

        private void Initialize()
        {
            _configurations = new ConcurrentDictionary<string, string>();
        }

        #region Loading Configurations

        public void LoadConfiguration(NameValueCollection nameValueCollection)
        {
            NameValueCollection nv = nameValueCollection;

            foreach (string key in nv)
            {
                _configurations.TryAdd(key, ReplaceVars(nv[key]));
            }
            //
            var env = Environment.GetEnvironmentVariables();
            foreach (string item in env.Keys)
            {
                _configurations.TryAdd(item, ReplaceVars(env[item].ToString()));
            }
        }

        public void Configure(string configFilePath)
        {
            Console.WriteLine($"ConfigurationFile:{configFilePath}");

            if (File.Exists(configFilePath))
            {
                FileInfo configFile = new FileInfo(configFilePath);
                ConfigManager.Current.Configure(configFile);
                Console.WriteLine($"Configuration loaded, config data count:{ConfigManager.Current.Configurations.Count}");
            }
            else
            {
                Console.WriteLine($"Configuration not found path:{configFilePath}");
            }
        }
        public void Configure(FileInfo configFileInfo)
        {
            if (configFileInfo == null)
            {
                Console.WriteLine("configFileInfo does not exist!");
                System.Diagnostics.Debug.WriteLine("configFileInfo does not exist!");
                //return;
                throw new Exception("configFileInfo does not exist!");
            }
            if (!configFileInfo.Exists)
            {
                Console.WriteLine("configFileInfo path does not exist!");
                System.Diagnostics.Debug.WriteLine("configFileInfo path does not exist!");
                //return;
                throw new Exception("configFileInfo path does not exist!");
            }

            NameValueCollection nvc = ReadNameValueXml("configuration", configFileInfo.FullName);
            LoadConfiguration(nvc);
        }

        #endregion Loading Configurations

        #region Get generic config

        public static Tp Get<Tp>(string key) where Tp : class
        {
            return ConfigManager.Current.GetConfig<Tp>(key);
        }

        public static Tp Get<Tp>(string key, Tp defaultVal) where Tp : class
        {
            return ConfigManager.Current.GetConfig<Tp>(key, defaultVal);
        }

        public T GetConfig<T>(string key) where T : class
        {
            return GetConfig<T>(key, null);
        }
        public T GetConfig<T>(string key, T defaultVal)
        {
            if (_configurations.ContainsKey(key))
            {
                string result = _configurations[key];
                result = ReplaceVars(result);
                return (T)System.Convert.ChangeType(result, typeof(T));
            }
            return defaultVal;
        }

        private string ReplaceVars(string str)
        {
            foreach (string key in _configurations.Keys)
            {
                string val = _configurations[key];
                if (str.Contains("${" + key + "}"))
                {
                    str = str.Replace("${" + key + "}", val);
                }
            }
            return str;
        }

        #endregion Get generic config

        #region File reload monitor

        //TODO: atilla when configuration loading, it gets file modified date
        //every one minute one thread compare filemodification date, and decide reloading

        #endregion File reload monitor

        #region helpers
        public static NameValueCollection ReadNameValueXml(string rootNodeName, string xmlPath)
        {
            /* Example xml file
             <?xml version="1.0" encoding="utf-8" standalone="yes"?>
                <configurations>
                    <config key="testKey" value="testValue"/>
                </configurations>
             */
            NameValueCollection nameValueCollection = new NameValueCollection();
            string path = xmlPath;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode node = doc.SelectSingleNode("//" + rootNodeName);
                foreach (XmlNode item in node.ChildNodes)
                {
                    if (item.NodeType != XmlNodeType.Comment)
                    {
                        string key = item.Attributes[0].Value;
                        string val = item.Attributes[1].Value;
                        nameValueCollection.Add(key, val);
                    }
                }
            }
            catch (System.IO.FileNotFoundException e)
            {
                throw new Exception("Xml File not found! or Xml file can not read!", e);
            }
            return nameValueCollection;
        }
        #endregion
    }
}