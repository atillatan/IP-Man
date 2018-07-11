/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by:   Atilla Tanrikulu 
 * @Last Modified time: 2018-04-16 10:10:45 
 */
using Core.Framework.Service;
using DeviceManager.Service.Service;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManager.API.Controllers
{
    public class BaseAPIController<T> : Controller
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(T));

        public ServiceContext ServiceContext
        {
            get { return (ServiceContext)HttpContext.Items["ServiceContext"]; }
        }

        private ServiceManager sm;

        public ServiceManager Sm
        {
            get
            {
                if (sm == null)
                    sm = new ServiceManager(ServiceContext);
                return sm;
            }
        }
    }
}