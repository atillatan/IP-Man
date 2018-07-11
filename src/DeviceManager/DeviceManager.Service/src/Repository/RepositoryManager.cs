/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by: Atilla Tanrikulu
 * @Last Modified time: 2018-05-02 18:19:18
 */
using System.Reflection;
using Core.Framework.Repository;
using Core.Framework.Service;

namespace DeviceManager.Service.Repository
{
    public class RepositoryManager : BaseRepositoryManager
    {
        public RepositoryManager(ServiceContext serviceContext) : base(serviceContext)
        { }

        #region SettingsRepository
        private ISettingsRepository _settingsRepository;

        public ISettingsRepository SettingsRepository
        {
            get
            {
                if (this._settingsRepository == null)
                {
                    switch (ServiceContext.GetItem<DatabaseContext>("DatabaseContext").DbProviderName)
                    {
                        case "System.Data.PostgreSQL":
                            //this._settingsRepository = new PostgreSQL.SettingsRepository(DatabaseContext, ServiceContext);
                            break;
                        default://System.Data.SQLite
                            this._settingsRepository = new SQLite.SettingsRepository(DatabaseContext, ServiceContext);
                            break;
                    }

                }

                return _settingsRepository;
            }
        }
        #endregion  SettingsRepository

        #region LanguageRepository
        private ILanguageRepository _languageRepository;

        public ILanguageRepository LanguageRepository
        {
            get
            {
                if (this._languageRepository == null)
                {
                    switch (ServiceContext.GetItem<DatabaseContext>("DatabaseContext").DbProviderName)
                    {
                        case "System.Data.PostgreSQL":
                            //this._languageRepository = new PostgreSQL.LanguageRepository(DatabaseContext, ServiceContext);
                            break;
                        default://System.Data.SQLite
                            this._languageRepository = new SQLite.LanguageRepository(DatabaseContext, ServiceContext);
                            break;
                    }

                }

                return _languageRepository;
            }
        }
        #endregion  LanguageRepository

        #region TemplateRepository
        private ITemplateRepository _templateRepository;

        public ITemplateRepository TemplateRepository
        {
            get
            {
                if (this._templateRepository == null)
                {
                    switch (ServiceContext.GetItem<DatabaseContext>("DatabaseContext").DbProviderName)
                    {
                        case "System.Data.PostgreSQL":
                            //this._templateRepository = new PostgreSQL.TemplateRepository(DatabaseContext, ServiceContext);
                            break;
                        default://System.Data.SQLite
                            this._templateRepository = new SQLite.TemplateRepository(DatabaseContext, ServiceContext);
                            break;
                    }

                }

                return _templateRepository;
            }
        }
        #endregion  TemplateRepository

        #region TemplatePropertyRepository
        private ITemplatePropertyRepository _templatePropertyRepository;

        public ITemplatePropertyRepository TemplatePropertyRepository
        {
            get
            {
                if (this._templatePropertyRepository == null)
                {
                    switch (ServiceContext.GetItem<DatabaseContext>("DatabaseContext").DbProviderName)
                    {
                        case "System.Data.PostgreSQL":
                            //this._templatePropertyRepository = new PostgreSQL.TemplatePropertyRepository(DatabaseContext, ServiceContext);
                            break;
                        default://System.Data.SQLite
                            this._templatePropertyRepository = new SQLite.TemplatePropertyRepository(DatabaseContext, ServiceContext);
                            break;
                    }

                }

                return _templatePropertyRepository;
            }
        }
        #endregion  TemplatePropertyRepository

        #region DeviceRepository
        private IDeviceRepository _deviceRepository;

        public IDeviceRepository DeviceRepository
        {
            get
            {
                if (this._deviceRepository == null)
                {
                    switch (ServiceContext.GetItem<DatabaseContext>("DatabaseContext").DbProviderName)
                    {
                        case "System.Data.PostgreSQL":
                            //this._deviceRepository = new PostgreSQL.DeviceRepository(DatabaseContext, ServiceContext);
                            break;
                        default://System.Data.SQLite
                            this._deviceRepository = new SQLite.DeviceRepository(DatabaseContext, ServiceContext);
                            break;
                    }

                }

                return _deviceRepository;
            }
        }
        #endregion  DeviceRepository

        #region DevicePropertyRepository
        private IDevicePropertyRepository _devicePropertyRepository;

        public IDevicePropertyRepository DevicePropertyRepository
        {
            get
            {
                if (this._devicePropertyRepository == null)
                {
                    switch (ServiceContext.GetItem<DatabaseContext>("DatabaseContext").DbProviderName)
                    {
                        case "System.Data.PostgreSQL":
                            //this._devicePropertyRepository = new PostgreSQL.DevicePropertyRepository(DatabaseContext, ServiceContext);
                            break;
                        default://System.Data.SQLite
                            this._devicePropertyRepository = new SQLite.DevicePropertyRepository(DatabaseContext, ServiceContext);
                            break;
                    }

                }

                return _devicePropertyRepository;
            }
        }
        #endregion  DevicePropertyRepository

        #region PropertyRepository
        private IPropertyRepository _propertyRepository;

        public IPropertyRepository PropertyRepository
        {
            get
            {
                if (this._propertyRepository == null)
                {
                    switch (ServiceContext.GetItem<DatabaseContext>("DatabaseContext").DbProviderName)
                    {
                        case "System.Data.PostgreSQL":
                            //this._propertyRepository = new PostgreSQL.PropertyRepository(DatabaseContext, ServiceContext);
                            break;
                        default://System.Data.SQLite
                            this._propertyRepository = new SQLite.PropertyRepository(DatabaseContext, ServiceContext);
                            break;
                    }

                }

                return _propertyRepository;
            }
        }
        #endregion  PropertyRepository

        #region ModelRepository
        private IModelRepository _modelRepository;

        public IModelRepository ModelRepository
        {
            get
            {
                if (this._modelRepository == null)
                {
                    switch (ServiceContext.GetItem<DatabaseContext>("DatabaseContext").DbProviderName)
                    {
                        case "System.Data.PostgreSQL":
                            //this._modelRepository = new PostgreSQL.ModelRepository(DatabaseContext, ServiceContext);
                            break;
                        default://System.Data.SQLite
                            this._modelRepository = new SQLite.ModelRepository(DatabaseContext, ServiceContext);
                            break;
                    }

                }

                return _modelRepository;
            }
        }
        #endregion  ModelRepository


    }
}