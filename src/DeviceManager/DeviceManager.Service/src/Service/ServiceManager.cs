/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:09:08 
 * @Last Modified by: Atilla Tanrikulu
 * @Last Modified time: 2018-05-02 18:20:20
 */

using System;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Framework.Middleware;
using Core.Framework.Service;


namespace DeviceManager.Service.Service
{
    public class ServiceManager : BaseServiceManager
    {
        public ServiceManager(ServiceContext serviceContext) : base(serviceContext)
        { }

        #region SystemService

        private ProxyService<SystemService> _sistemService;

        public ProxyService<SystemService> SystemService
        {
            get
            {
                if (_sistemService == null)
                    _sistemService = new ProxyService<SystemService>(new SystemService(ServiceContext));
                return _sistemService;
            }
        }

        #endregion SystemService

        #region NetworkService

        private ProxyService<NetworkService> _networkService;

        public ProxyService<NetworkService> NetworkService
        {
            get
            {
                if (_networkService == null)
                    _networkService = new ProxyService<NetworkService>(new NetworkService(ServiceContext));
                return _networkService;
            }
        }

        #endregion NetworkService

        #region DeviceService

        private ProxyService<DeviceService> _deviceService;

        public ProxyService<DeviceService> DeviceService
        {
            get
            {
                if (_deviceService == null)
                    _deviceService = new ProxyService<DeviceService>(new DeviceService(ServiceContext));
                return _deviceService;
            }
        }

        #endregion DeviceService

    }
}