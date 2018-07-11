/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:24 
 * @Last Modified by:   Atilla Tanrikulu 
 * @Last Modified time: 2018-04-16 10:10:24 
 */

using System;
using System.Data.Common;
using Core.Framework.Service;

namespace Core.Framework.Repository
{
    public class BaseRepositoryManager : IDisposable
    {
        protected DatabaseContext DatabaseContext = null;

        protected ServiceContext ServiceContext = null;

        public BaseRepositoryManager(ServiceContext serviceContext)
        {
            ServiceContext = serviceContext;
            var DatabaseContext = serviceContext.GetItem<DatabaseContext>("DatabaseContext");
            this.DatabaseContext = DatabaseContext;
        }

        public void Commit()
        {
            try
            {
                DatabaseContext.Commit();
            }
            catch (Exception e)
            {
                //Log.Exception(e);
                throw e;
            }
        }

        public void RollBack()
        {
            try
            {
                DatabaseContext.Rollback();
            }
            catch (Exception e)
            {
                //Log.Exception(e);
                throw e;
            }
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
                    if (DatabaseContext != null)
                    {
                        //_log.Debug("DataBaseContext is being disposed");
                        DatabaseContext.Dispose();
                    }
                }
                disposed = true;
            }
        }

        ~BaseRepositoryManager()
        {
            Dispose(false);
        }

        #endregion Disposing
    }
}