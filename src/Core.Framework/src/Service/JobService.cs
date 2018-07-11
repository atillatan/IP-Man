using System;
using System.Collections.Concurrent;


namespace Core.Framework.Service
{
    public class JobService
    {
        public string ConfigPath { get; set; }
        public JobService(string configPath)
        {
            //TODO: atilla Quartz scheduler eklenecek
            this.ConfigPath = configPath;
        }
    }
}