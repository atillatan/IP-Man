/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by: Atilla Tanrikulu
 * @Last Modified time: 2018-05-02 17:56:50
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DeviceManager.Service.Tests
{
    public class Startup : IDisposable
    {
        public Startup()
        {

            DeviceManager.Service.Startup.Start($"../../../.config/devicemanager/devicemanager.development.config");
        }

        public void Dispose()
        {
            Console.WriteLine("... clean up test data from the database ...");
        }
    }

    [CollectionDefinition("ServiceCollection")]
    public class ServiceCollection : ICollectionFixture<Startup>
    {
    }
}