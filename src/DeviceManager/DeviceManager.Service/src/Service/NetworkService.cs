/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by: Atilla Tanrikulu
 * @Last Modified time: 2018-05-02 18:21:18
 */
using System;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using log4net;
using Core.Framework.Service;
using Core.Framework.Repository.DTO;
using Core.Framework.Middleware;
using Core.Framework;
using DeviceManager.Service.Repository;
using DeviceManager.Service.Entity.System;
using DeviceManager.DTO.System;
using System.Threading;
using Core.Framework.Util;
using System.Collections.Concurrent;
using DeviceManager.Service.Util;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using DynamicDbUtil;

namespace DeviceManager.Service.Service
{
    // [Authorized]
    public class NetworkService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(NetworkService));

        public ServiceContext ServiceContext { get; set; }
        public NetworkService(ServiceContext serviceContext)
        {
            this.ServiceContext = serviceContext;
        }

        public LanguageService LanguageService = Application.Current.GetService<LanguageService>();
        public string Translate(string key) => LanguageService.Translate(key, ServiceContext.UserInfo.Language);

        private static int activePingCompletedEventHandler = 0;
        /// <summary>
        /// DiscoverNetwork
        /// </summary>
        /// <returns></returns>
        [Transactional(false)]
        public ServiceResponse<Dictionary<string, object>> DiscoverNetwork()
        {
            List<string> gatewayList = NetworkUtil.GetInterfaceIps();

            for (int g = 0; g < gatewayList.Count; g++)
            {
                string[] gatewayIpSegments = gatewayList[g].Split('.');
                for (int i = 1; i <= 255; i++)
                {

                    string ip = gatewayIpSegments[0] + "." + gatewayIpSegments[1] + "." + gatewayIpSegments[2] + "." + i;
                    Ping ping = new Ping();
                    ping.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);

                    try
                    {
                        ping.SendAsync(ip, 1000, ip);
                        Interlocked.Increment(ref activePingCompletedEventHandler);
                    }
                    catch (Exception ex1)
                    {
                        Log.Debug($"ping try-1 failed for {ip}, reason: {ex1.Message}");
                        Interlocked.Decrement(ref activePingCompletedEventHandler);
                        try
                        {
                            Thread.Sleep(100);
                            ping.SendAsync(ip, 1000, ip);
                            Interlocked.Increment(ref activePingCompletedEventHandler);
                        }
                        catch (Exception ex2)
                        {
                            Log.Debug($"ping try-2 failed for {ip}, reason: {ex2.Message}");
                            Interlocked.Decrement(ref activePingCompletedEventHandler);
                            try
                            {
                                Thread.Sleep(100);
                                ping.SendAsync(ip, 1000, ip);
                                Interlocked.Increment(ref activePingCompletedEventHandler);
                            }
                            catch (Exception ex3)
                            {
                                Log.Debug($"ping try-3 failed for {ip}, reason: {ex3.Message}");
                                Interlocked.Decrement(ref activePingCompletedEventHandler);
                                Log.Debug($"tryed 3 times for:{ip} {ex3.Message}");
                            }
                        }
                    }
                }
            }

            //wait all pings
            while (true)
            {
                Log.Debug($"UnknownDevice.count:{activePingCompletedEventHandler} Device.count:{CacheUtil.Cache("DEVICES").Count}");
                if (activePingCompletedEventHandler == 0) break;
                Thread.Sleep(1000);
            }

            var result = (ConcurrentDictionary<string, object>)CacheUtil.Cache("DEVICES");

            if (result.Count > 0)
                return new ServiceResponse<Dictionary<string, object>>(result.ToDictionary(x => x.Key, x => x.Value), Translate(MessagesConstants.SCC_DATA_DELETED));
            throw new ServiceException(Translate(MessagesConstants.ERR_DELETE));
        }



        /// <summary>
        /// GetDevicesFromCache
        /// </summary>
        /// <returns></returns>
        [Transactional(false)]
        public ServiceResponse<Dictionary<string, object>> GetDevicesFromCache()
        {

            var result = (ConcurrentDictionary<string, object>)CacheUtil.Cache("DEVICES");

            if (result.Count > 0)
                return new ServiceResponse<Dictionary<string, object>>(result.ToDictionary(x => x.Key, x => x.Value), Translate(MessagesConstants.SCC_DATA_DELETED));
            throw new ServiceException(Translate(MessagesConstants.ERR_DELETE));
        }

        #region private methods

        private static void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            try
            {
                string ip = (string)e.UserState;
                if (e.Reply != null && e.Reply.Status == IPStatus.Success)
                {
                    string macaddres = NetworkUtil.GetMacAddress(ip);
                    var deviceProperties = new ConcurrentDictionary<string, string>();
                    deviceProperties.TryAdd("IP", ip);
                    deviceProperties.TryAdd("MACADDRESS", NetworkUtil.GetMacAddress(ip));
                    CacheUtil.Cache("DEVICES")[macaddres] = deviceProperties;

                    LoadHostName(macaddres);
                    LoadVendor(macaddres);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                Interlocked.Decrement(ref activePingCompletedEventHandler);
            }
        }

        private static void LoadHostName(string macaddres)
        {
            ConcurrentDictionary<string, string> deviceProperties = (ConcurrentDictionary<string, string>)CacheUtil.Cache("DEVICES")[macaddres];

            string hostname = NetworkUtil.GetHostName(deviceProperties["IP"]);
            deviceProperties.TryAdd("HOSTNAME", hostname);
            //update
            CacheUtil.Cache("DEVICES")[macaddres] = deviceProperties;

        }

        private static void LoadVendor(string macaddres)
        {
            ConcurrentDictionary<string, string> deviceProperties = (ConcurrentDictionary<string, string>)CacheUtil.Cache("DEVICES")[macaddres];

            string vendor = "";

            using (DbConnection connection = new SqliteConnection(ConfigManager.Get<string>("database.vendor.connectionString")))
            {
                string macAddress = macaddres.Replace("-", "").Substring(0, 6).ToUpper();
                vendor = connection.Get($"select OrganizationName from oui where Assignment='{0}'", macAddress);
            }

            deviceProperties.TryAdd("VENDOR", vendor);
            //update
            CacheUtil.Cache("DEVICES")[macaddres] = deviceProperties;

        }

        #endregion
    }

}