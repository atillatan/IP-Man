/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by: Atilla Tanrikulu
 * @Last Modified time: 2018-05-02 18:25:30
 */
using System.Linq;
using Core.Framework.Service;
using Core.Framework.Repository.DTO;
using DeviceManager.DTO.System;
using DeviceManager.Service.Service;
using Xunit;
using System.Threading;

namespace DeviceManager.Service.Tests
{
    public class NetworkServiceTest : TestBase
    {
        [Fact]
        public void DicoverNetwork_CRUD()
        {
            var proxy = new ProxyService<NetworkService>(new NetworkService(new ServiceContext()));

            var rp = proxy.RunAsync(x => x.DiscoverNetwork());
            rp.Wait();

            Assert.True(rp.Result.Data != null);

        }

 
    }
}