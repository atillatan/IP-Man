/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by:   Atilla Tanrikulu 
 * @Last Modified time: 2018-04-16 10:10:45 
 */
using System;

namespace Core.Framework.Service
{
    public class BaseServiceManager : IDisposable
    {
        private readonly ServiceContext _serviceContext = null;

        public ServiceContext ServiceContext
        {
            get { return _serviceContext; }
        }

        public BaseServiceManager(ServiceContext serviceContext)
        {
            
            _serviceContext = serviceContext;
        }

        #region Disposing

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (_serviceContext != null)
                    {
                        //_log.Debug("ServiceContext is being disposed");
                        _serviceContext.Dispose();
                    }
                }
                disposed = true;
            }
        }

        ~BaseServiceManager()
        {
            Dispose(false);
        }

        #endregion Disposing
    }
}