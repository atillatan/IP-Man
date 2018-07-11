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
using DeviceManager.DTO;
using DeviceManager.Service.Entity;

namespace DeviceManager.Service.Service
{
     //[Authorized]
    public class DeviceService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DeviceService));

        public ServiceContext ServiceContext { get; set; }
        public DeviceService(ServiceContext serviceContext)
        {
            this.ServiceContext = serviceContext;
        }

        public LanguageService LanguageService = Application.Current.GetService<LanguageService>();
        public string Translate(string key) => LanguageService.Translate(key, ServiceContext.UserInfo.Language);

        #region Model

        /// <summary>
        /// Model kaydet
        /// </summary>
        /// <param name="ModelDto"></param>
        /// <returns></returns>
        [Transactional(true)]
        [Authorized("MODEL_INSERT")]
        public ServiceResponse<long> InsertModel(ModelDto modelDto)
        {
            if (modelDto == null) throw new ServiceException(Translate(MessagesConstants.ERR_DATA_NOT_FOUND_TO_SAVE));

            Model entity = new Model();
            var Rm = new RepositoryManager(ServiceContext);
            long id = Rm.ModelRepository.Insert(entity.CopyFrom(modelDto));
            if (id > 0)
                return new ServiceResponse<long>(id, Translate(MessagesConstants.SCC_DATA_INSERTED));
            throw new ServiceException(Translate(MessagesConstants.ERR_INSERT));
        }

        /// <summary>
        /// Id si verilen Device getirir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Transactional(false)]
        [Authorized("MODEL_GET")]
        public ServiceResponse<ModelDto> GetModel(long id)
        {
            var Rm = new RepositoryManager(ServiceContext);
            Model entity = Rm.ModelRepository.Get(id);
            if (entity == null)
                throw new ServiceException(ExceptionType.Warning, Translate(MessagesConstants.WRN_RECORD_NOT_FOUND));
            return new ServiceResponse<ModelDto>(entity.CopyTo(new ModelDto()));
        }

        /// <summary>
        /// verilen ModelDto yu gunceller
        /// </summary>
        /// <param name="modelDto"></param>
        /// <returns></returns>
        [Transactional(true)]
        [Authorized("MODEL_UPDATE")]
        public ServiceResponse<int> UpdateModel(ModelDto modelDto)
        {
            if (modelDto == null) throw new ServiceException(Translate(MessagesConstants.ERR_DATA_NOT_FOUND_TO_SAVE));

            Model entity = new Model();
            var Rm = new RepositoryManager(ServiceContext);
            int rowsAffected = Rm.ModelRepository.Update(entity.CopyFrom(modelDto));

            if (rowsAffected > 0)
                return new ServiceResponse<int>(rowsAffected, Translate(MessagesConstants.SCC_DATA_UPDATED));
            throw new ServiceException(Translate(MessagesConstants.ERR_UPDATE));
        }

        /// <summary>
        /// Sistemden language siler
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Transactional(true)]
        [Authorized("MODEL_DELETE")]
        public ServiceResponse<int> DeleteModel(long id)
        {
            var Rm = new RepositoryManager(ServiceContext);
            int rowsAffected = Rm.ModelRepository.Delete(id);
            if (rowsAffected > 0)
                return new ServiceResponse<int>(rowsAffected, Translate(MessagesConstants.SCC_DATA_DELETED));
            throw new ServiceException(Translate(MessagesConstants.ERR_DELETE));
        }

        /// <summary>
        /// ModelDto verilerine gore sorgualama yapar
        /// </summary>
        /// <param name="modelDto"></param>
        /// <returns></returns>
        [Transactional(false)]
        [Authorized("MODEL_LIST")]
        public ServiceResponse<IEnumerable<ModelDto>> ListModel(ModelDto modelDto, PagingDto pagingDto)
        {
            var Rm = new RepositoryManager(ServiceContext);
            IEnumerable<Model> list = Rm.ModelRepository.List(modelDto, ref pagingDto);
            IEnumerable<ModelDto> restulList = list.Select(entity => entity.CopyTo(new ModelDto())).ToList();

            if (list == null || !list.Any())
                throw new ServiceException(ExceptionType.Warning, Translate(MessagesConstants.WRN_RECORD_NOT_FOUND));
            return new ServiceResponse<IEnumerable<ModelDto>>(restulList, pagingDto.count);
        }

        /// <summary>
        /// Sistemden language siler
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Transactional(true)]
        [Authorized("MODEL_DELETE_HARD")]
        public ServiceResponse<int> DeleteHardModel(long id)
        {
            var Rm = new RepositoryManager(ServiceContext);
            int rowsAffected = Rm.ModelRepository.DeleteHard(id);
            if (rowsAffected > 0)
                return new ServiceResponse<int>(rowsAffected, Translate(MessagesConstants.SCC_DATA_DELETED));
            throw new ServiceException(Translate(MessagesConstants.ERR_DELETE));
        }

        /// <summary>
        /// ModelDto verilerine gore sorgualama yapar
        /// </summary>
        /// <param name="modelDto"></param>
        /// <returns></returns>
        [Transactional(false)]
        [Authorized("UNAUTHORIZED")]
        public ServiceResponse<IEnumerable<ModelDto>> ListAllModel()
        {
            var Rm = new RepositoryManager(ServiceContext);
            IEnumerable<Model> list = Rm.ModelRepository.ListAll();
            IEnumerable<ModelDto> restulList = list.Select(entity => entity.CopyTo(new ModelDto())).ToList();

            if (list == null || !list.Any())
                throw new ServiceException(ExceptionType.Warning, Translate(MessagesConstants.WRN_RECORD_NOT_FOUND));
            return new ServiceResponse<IEnumerable<ModelDto>>(restulList);
        }

        #endregion Model

        #region Property

        /// <summary>
        /// Property kaydet
        /// </summary>
        /// <param name="PropertyDto"></param>
        /// <returns></returns>
        [Transactional(true)]
        [Authorized("PROPERTY_INSERT")]
        public ServiceResponse<long> InsertProperty(PropertyDto propertyDto)
        {
            if (propertyDto == null) throw new ServiceException(Translate(MessagesConstants.ERR_DATA_NOT_FOUND_TO_SAVE));

            Property entity = new Property();
            var Rm = new RepositoryManager(ServiceContext);
            long id = Rm.PropertyRepository.Insert(entity.CopyFrom(propertyDto));
            if (id > 0)
                return new ServiceResponse<long>(id, Translate(MessagesConstants.SCC_DATA_INSERTED));
            throw new ServiceException(Translate(MessagesConstants.ERR_INSERT));
        }

        /// <summary>
        /// Id si verilen Device getirir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Transactional(false)]
        [Authorized("PROPERTY_GET")]
        public ServiceResponse<PropertyDto> GetProperty(long id)
        {
            var Rm = new RepositoryManager(ServiceContext);
            Property entity = Rm.PropertyRepository.Get(id);
            if (entity == null)
                throw new ServiceException(ExceptionType.Warning, Translate(MessagesConstants.WRN_RECORD_NOT_FOUND));
            return new ServiceResponse<PropertyDto>(entity.CopyTo(new PropertyDto()));
        }

        /// <summary>
        /// verilen PropertyDto yu gunceller
        /// </summary>
        /// <param name="propertyDto"></param>
        /// <returns></returns>
        [Transactional(true)]
        [Authorized("PROPERTY_UPDATE")]
        public ServiceResponse<int> UpdateProperty(PropertyDto propertyDto)
        {
            if (propertyDto == null) throw new ServiceException(Translate(MessagesConstants.ERR_DATA_NOT_FOUND_TO_SAVE));

            Property entity = new Property();
            var Rm = new RepositoryManager(ServiceContext);
            int rowsAffected = Rm.PropertyRepository.Update(entity.CopyFrom(propertyDto));

            if (rowsAffected > 0)
                return new ServiceResponse<int>(rowsAffected, Translate(MessagesConstants.SCC_DATA_UPDATED));
            throw new ServiceException(Translate(MessagesConstants.ERR_UPDATE));
        }

        /// <summary>
        /// Sistemden language siler
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Transactional(true)]
        [Authorized("PROPERTY_DELETE")]
        public ServiceResponse<int> DeleteProperty(long id)
        {
            var Rm = new RepositoryManager(ServiceContext);
            int rowsAffected = Rm.PropertyRepository.Delete(id);
            if (rowsAffected > 0)
                return new ServiceResponse<int>(rowsAffected, Translate(MessagesConstants.SCC_DATA_DELETED));
            throw new ServiceException(Translate(MessagesConstants.ERR_DELETE));
        }

        /// <summary>
        /// PropertyDto verilerine gore sorgualama yapar
        /// </summary>
        /// <param name="propertyDto"></param>
        /// <returns></returns>
        [Transactional(false)]
        [Authorized("PROPERTY_LIST")]
        public ServiceResponse<IEnumerable<PropertyDto>> ListProperty(PropertyDto propertyDto, PagingDto pagingDto)
        {
            var Rm = new RepositoryManager(ServiceContext);
            IEnumerable<Property> list = Rm.PropertyRepository.List(propertyDto, ref pagingDto);
            IEnumerable<PropertyDto> restulList = list.Select(entity => entity.CopyTo(new PropertyDto())).ToList();

            if (list == null || !list.Any())
                throw new ServiceException(ExceptionType.Warning, Translate(MessagesConstants.WRN_RECORD_NOT_FOUND));
            return new ServiceResponse<IEnumerable<PropertyDto>>(restulList, pagingDto.count);
        }

        /// <summary>
        /// Sistemden language siler
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Transactional(true)]
        [Authorized("PROPERTY_DELETE_HARD")]
        public ServiceResponse<int> DeleteHardProperty(long id)
        {
            var Rm = new RepositoryManager(ServiceContext);
            int rowsAffected = Rm.PropertyRepository.DeleteHard(id);
            if (rowsAffected > 0)
                return new ServiceResponse<int>(rowsAffected, Translate(MessagesConstants.SCC_DATA_DELETED));
            throw new ServiceException(Translate(MessagesConstants.ERR_DELETE));
        }

        /// <summary>
        /// PropertyDto verilerine gore sorgualama yapar
        /// </summary>
        /// <param name="propertyDto"></param>
        /// <returns></returns>
        [Transactional(false)]
        [Authorized("UNAUTHORIZED")]
        public ServiceResponse<IEnumerable<PropertyDto>> ListAllProperty()
        {
            var Rm = new RepositoryManager(ServiceContext);
            IEnumerable<Property> list = Rm.PropertyRepository.ListAll();
            IEnumerable<PropertyDto> restulList = list.Select(entity => entity.CopyTo(new PropertyDto())).ToList();

            if (list == null || !list.Any())
                throw new ServiceException(ExceptionType.Warning, Translate(MessagesConstants.WRN_RECORD_NOT_FOUND));
            return new ServiceResponse<IEnumerable<PropertyDto>>(restulList);
        }

        #endregion Property

        #region Template

        /// <summary>
        /// Template kaydet
        /// </summary>
        /// <param name="TemplateDto"></param>
        /// <returns></returns>
        [Transactional(true)]
        [Authorized("TEMPLATE_INSERT")]
        public ServiceResponse<long> InsertTemplate(TemplateDto templateDto)
        {
            if (templateDto == null) throw new ServiceException(Translate(MessagesConstants.ERR_DATA_NOT_FOUND_TO_SAVE));

            Template templateEntity = new Template();
            var Rm = new RepositoryManager(ServiceContext);
            long id = Rm.TemplateRepository.Insert(templateEntity.CopyFrom(templateDto));
            if (id > 0)
                return new ServiceResponse<long>(id, Translate(MessagesConstants.SCC_DATA_INSERTED));
            throw new ServiceException(Translate(MessagesConstants.ERR_INSERT));
        }

        /// <summary>
        /// Id si verilen Device getirir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Transactional(false)]
        [Authorized("TEMPLATE_GET")]
        public ServiceResponse<TemplateDto> GetTemplate(long id)
        {
            var Rm = new RepositoryManager(ServiceContext);
            Template templateEntity = Rm.TemplateRepository.Get(id);
            if (templateEntity == null)
                throw new ServiceException(ExceptionType.Warning, Translate(MessagesConstants.WRN_RECORD_NOT_FOUND));
            return new ServiceResponse<TemplateDto>(templateEntity.CopyTo(new TemplateDto()));
        }

        /// <summary>
        /// verilen TemplateDto yu gunceller
        /// </summary>
        /// <param name="templateDto"></param>
        /// <returns></returns>
        [Transactional(true)]
        [Authorized("TEMPLATE_UPDATE")]
        public ServiceResponse<int> UpdateTemplate(TemplateDto templateDto)
        {
            if (templateDto == null) throw new ServiceException(Translate(MessagesConstants.ERR_DATA_NOT_FOUND_TO_SAVE));

            Template templateEntity = new Template();
            var Rm = new RepositoryManager(ServiceContext);
            int rowsAffected = Rm.TemplateRepository.Update(templateEntity.CopyFrom(templateDto));

            if (rowsAffected > 0)
                return new ServiceResponse<int>(rowsAffected, Translate(MessagesConstants.SCC_DATA_UPDATED));
            throw new ServiceException(Translate(MessagesConstants.ERR_UPDATE));
        }

        /// <summary>
        /// Sistemden language siler
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Transactional(true)]
        [Authorized("TEMPLATE_DELETE")]
        public ServiceResponse<int> DeleteTemplate(long id)
        {
            var Rm = new RepositoryManager(ServiceContext);
            int rowsAffected = Rm.TemplateRepository.Delete(id);
            if (rowsAffected > 0)
                return new ServiceResponse<int>(rowsAffected, Translate(MessagesConstants.SCC_DATA_DELETED));
            throw new ServiceException(Translate(MessagesConstants.ERR_DELETE));
        }

        /// <summary>
        /// TemplateDto verilerine gore sorgualama yapar
        /// </summary>
        /// <param name="templateDto"></param>
        /// <returns></returns>
        [Transactional(false)]
        [Authorized("TEMPLATE_LIST")]
        public ServiceResponse<IEnumerable<TemplateDto>> ListTemplate(TemplateDto templateDto, PagingDto pagingDto)
        {
            var Rm = new RepositoryManager(ServiceContext);
            IEnumerable<Template> list = Rm.TemplateRepository.List(templateDto, ref pagingDto);
            IEnumerable<TemplateDto> restulList = list.Select(entity => entity.CopyTo(new TemplateDto())).ToList();

            if (list == null || !list.Any())
                throw new ServiceException(ExceptionType.Warning, Translate(MessagesConstants.WRN_RECORD_NOT_FOUND));
            return new ServiceResponse<IEnumerable<TemplateDto>>(restulList, pagingDto.count);
        }

        /// <summary>
        /// Sistemden language siler
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Transactional(true)]
        [Authorized("TEMPLATE_DELETE_HARD")]
        public ServiceResponse<int> DeleteHardTemplate(long id)
        {
            var Rm = new RepositoryManager(ServiceContext);
            int rowsAffected = Rm.TemplateRepository.DeleteHard(id);
            if (rowsAffected > 0)
                return new ServiceResponse<int>(rowsAffected, Translate(MessagesConstants.SCC_DATA_DELETED));
            throw new ServiceException(Translate(MessagesConstants.ERR_DELETE));
        }

        /// <summary>
        /// TemplateDto verilerine gore sorgualama yapar
        /// </summary>
        /// <param name="templateDto"></param>
        /// <returns></returns>
        [Transactional(false)]
        [Authorized("UNAUTHORIZED")]
        public ServiceResponse<IEnumerable<TemplateDto>> ListAllTemplate()
        {
            var Rm = new RepositoryManager(ServiceContext);
            IEnumerable<Template> list = Rm.TemplateRepository.ListAll();
            IEnumerable<TemplateDto> restulList = list.Select(entity => entity.CopyTo(new TemplateDto())).ToList();

            if (list == null || !list.Any())
                throw new ServiceException(ExceptionType.Warning, Translate(MessagesConstants.WRN_RECORD_NOT_FOUND));
            return new ServiceResponse<IEnumerable<TemplateDto>>(restulList);
        }

        #endregion Template

        #region TemplateProperty

        /// <summary>
        /// TemplateProperty kaydet
        /// </summary>
        /// <param name="TemplatePropertyDto"></param>
        /// <returns></returns>
        [Transactional(true)]
        [Authorized("TEMPLATEPROPERTY_INSERT")]
        public ServiceResponse<long> InsertTemplateProperty(TemplatePropertyDto templatePropertyDto)
        {
            if (templatePropertyDto == null) throw new ServiceException(Translate(MessagesConstants.ERR_DATA_NOT_FOUND_TO_SAVE));

            TemplateProperty entity = new TemplateProperty();
            var Rm = new RepositoryManager(ServiceContext);
            long id = Rm.TemplatePropertyRepository.Insert(entity.CopyFrom(templatePropertyDto));
            if (id > 0)
                return new ServiceResponse<long>(id, Translate(MessagesConstants.SCC_DATA_INSERTED));
            throw new ServiceException(Translate(MessagesConstants.ERR_INSERT));
        }

        /// <summary>
        /// Id si verilen Device getirir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Transactional(false)]
        [Authorized("TEMPLATEPROPERTY_GET")]
        public ServiceResponse<TemplatePropertyDto> GetTemplateProperty(long id)
        {
            var Rm = new RepositoryManager(ServiceContext);
            TemplateProperty entity = Rm.TemplatePropertyRepository.Get(id);
            if (entity == null)
                throw new ServiceException(ExceptionType.Warning, Translate(MessagesConstants.WRN_RECORD_NOT_FOUND));
            return new ServiceResponse<TemplatePropertyDto>(entity.CopyTo(new TemplatePropertyDto()));
        }

        /// <summary>
        /// verilen TemplatePropertyDto yu gunceller
        /// </summary>
        /// <param name="templatePropertyDto"></param>
        /// <returns></returns>
        [Transactional(true)]
        [Authorized("TEMPLATEPROPERTY_UPDATE")]
        public ServiceResponse<int> UpdateTemplateProperty(TemplatePropertyDto templatePropertyDto)
        {
            if (templatePropertyDto == null) throw new ServiceException(Translate(MessagesConstants.ERR_DATA_NOT_FOUND_TO_SAVE));

            TemplateProperty entity = new TemplateProperty();
            var Rm = new RepositoryManager(ServiceContext);
            int rowsAffected = Rm.TemplatePropertyRepository.Update(entity.CopyFrom(templatePropertyDto));

            if (rowsAffected > 0)
                return new ServiceResponse<int>(rowsAffected, Translate(MessagesConstants.SCC_DATA_UPDATED));
            throw new ServiceException(Translate(MessagesConstants.ERR_UPDATE));
        }

        /// <summary>
        /// Sistemden language siler
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Transactional(true)]
        [Authorized("TEMPLATEPROPERTY_DELETE")]
        public ServiceResponse<int> DeleteTemplateProperty(long id)
        {
            var Rm = new RepositoryManager(ServiceContext);
            int rowsAffected = Rm.TemplatePropertyRepository.Delete(id);
            if (rowsAffected > 0)
                return new ServiceResponse<int>(rowsAffected, Translate(MessagesConstants.SCC_DATA_DELETED));
            throw new ServiceException(Translate(MessagesConstants.ERR_DELETE));
        }

        /// <summary>
        /// TemplatePropertyDto verilerine gore sorgualama yapar
        /// </summary>
        /// <param name="templatePropertyDto"></param>
        /// <returns></returns>
        [Transactional(false)]
        [Authorized("TEMPLATEPROPERTY_LIST")]
        public ServiceResponse<IEnumerable<TemplatePropertyDto>> ListTemplateProperty(TemplatePropertyDto templatePropertyDto, PagingDto pagingDto)
        {
            var Rm = new RepositoryManager(ServiceContext);
            IEnumerable<TemplateProperty> list = Rm.TemplatePropertyRepository.List(templatePropertyDto, ref pagingDto);
            IEnumerable<TemplatePropertyDto> restulList = list.Select(entity => entity.CopyTo(new TemplatePropertyDto())).ToList();

            if (list == null || !list.Any())
                throw new ServiceException(ExceptionType.Warning, Translate(MessagesConstants.WRN_RECORD_NOT_FOUND));
            return new ServiceResponse<IEnumerable<TemplatePropertyDto>>(restulList, pagingDto.count);
        }

        /// <summary>
        /// Sistemden language siler
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Transactional(true)]
        [Authorized("TEMPLATEPROPERTY_DELETE_HARD")]
        public ServiceResponse<int> DeleteHardTemplateProperty(long id)
        {
            var Rm = new RepositoryManager(ServiceContext);
            int rowsAffected = Rm.TemplatePropertyRepository.DeleteHard(id);
            if (rowsAffected > 0)
                return new ServiceResponse<int>(rowsAffected, Translate(MessagesConstants.SCC_DATA_DELETED));
            throw new ServiceException(Translate(MessagesConstants.ERR_DELETE));
        }

        /// <summary>
        /// TemplatePropertyDto verilerine gore sorgualama yapar
        /// </summary>
        /// <param name="templatePropertyDto"></param>
        /// <returns></returns>
        [Transactional(false)]
        [Authorized("UNAUTHORIZED")]
        public ServiceResponse<IEnumerable<TemplatePropertyDto>> ListAllTemplateProperty()
        {
            var Rm = new RepositoryManager(ServiceContext);
            IEnumerable<TemplateProperty> list = Rm.TemplatePropertyRepository.ListAll();
            IEnumerable<TemplatePropertyDto> restulList = list.Select(entity => entity.CopyTo(new TemplatePropertyDto())).ToList();

            if (list == null || !list.Any())
                throw new ServiceException(ExceptionType.Warning, Translate(MessagesConstants.WRN_RECORD_NOT_FOUND));
            return new ServiceResponse<IEnumerable<TemplatePropertyDto>>(restulList);
        }

        #endregion TemplateProperty

        #region Device

        /// <summary>
        /// Device kaydet
        /// </summary>
        /// <param name="DeviceDto"></param>
        /// <returns></returns>
        [Transactional(true)]
        [Authorized("DEVICE_INSERT")]
        public ServiceResponse<long> InsertDevice(DeviceDto deviceDto)
        {
            if (deviceDto == null) throw new ServiceException(Translate(MessagesConstants.ERR_DATA_NOT_FOUND_TO_SAVE));

            Device deviceEntity = new Device();
            var Rm = new RepositoryManager(ServiceContext);
            long id = Rm.DeviceRepository.Insert(deviceEntity.CopyFrom(deviceDto));
            if (id > 0)
                return new ServiceResponse<long>(id, Translate(MessagesConstants.SCC_DATA_INSERTED));
            throw new ServiceException(Translate(MessagesConstants.ERR_INSERT));
        }

        /// <summary>
        /// Id si verilen Device getirir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Transactional(false)]
        [Authorized("DEVICE_GET")]
        public ServiceResponse<DeviceDto> GetDevice(long id)
        {
            var Rm = new RepositoryManager(ServiceContext);
            Device deviceEntity = Rm.DeviceRepository.Get(id);
            if (deviceEntity == null)
                throw new ServiceException(ExceptionType.Warning, Translate(MessagesConstants.WRN_RECORD_NOT_FOUND));
            return new ServiceResponse<DeviceDto>(deviceEntity.CopyTo(new DeviceDto()));
        }

        /// <summary>
        /// verilen DeviceDto yu gunceller
        /// </summary>
        /// <param name="deviceDto"></param>
        /// <returns></returns>
        [Transactional(true)]
        [Authorized("DEVICE_UPDATE")]
        public ServiceResponse<int> UpdateDevice(DeviceDto deviceDto)
        {
            if (deviceDto == null) throw new ServiceException(Translate(MessagesConstants.ERR_DATA_NOT_FOUND_TO_SAVE));

            Device deviceEntity = new Device();
            var Rm = new RepositoryManager(ServiceContext);
            int rowsAffected = Rm.DeviceRepository.Update(deviceEntity.CopyFrom(deviceDto));

            if (rowsAffected > 0)
                return new ServiceResponse<int>(rowsAffected, Translate(MessagesConstants.SCC_DATA_UPDATED));
            throw new ServiceException(Translate(MessagesConstants.ERR_UPDATE));
        }

        /// <summary>
        /// Sistemden language siler
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Transactional(true)]
        [Authorized("DEVICE_DELETE")]
        public ServiceResponse<int> DeleteDevice(long id)
        {
            var Rm = new RepositoryManager(ServiceContext);
            int rowsAffected = Rm.DeviceRepository.Delete(id);
            if (rowsAffected > 0)
                return new ServiceResponse<int>(rowsAffected, Translate(MessagesConstants.SCC_DATA_DELETED));
            throw new ServiceException(Translate(MessagesConstants.ERR_DELETE));
        }

        /// <summary>
        /// DeviceDto verilerine gore sorgualama yapar
        /// </summary>
        /// <param name="deviceDto"></param>
        /// <returns></returns>
        [Transactional(false)]
        [Authorized("DEVICE_LIST")]
        public ServiceResponse<IEnumerable<DeviceDto>> ListDevice(DeviceDto deviceDto, PagingDto pagingDto)
        {
            var Rm = new RepositoryManager(ServiceContext);
            IEnumerable<Device> list = Rm.DeviceRepository.List(deviceDto, ref pagingDto);
            IEnumerable<DeviceDto> restulList = list.Select(entity => entity.CopyTo(new DeviceDto())).ToList();

            if (list == null || !list.Any())
                throw new ServiceException(ExceptionType.Warning, Translate(MessagesConstants.WRN_RECORD_NOT_FOUND));
            return new ServiceResponse<IEnumerable<DeviceDto>>(restulList, pagingDto.count);
        }

        /// <summary>
        /// Sistemden language siler
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Transactional(true)]
        [Authorized("DEVICE_DELETE_HARD")]
        public ServiceResponse<int> DeleteHardDevice(long id)
        {
            var Rm = new RepositoryManager(ServiceContext);
            int rowsAffected = Rm.DeviceRepository.DeleteHard(id);
            if (rowsAffected > 0)
                return new ServiceResponse<int>(rowsAffected, Translate(MessagesConstants.SCC_DATA_DELETED));
            throw new ServiceException(Translate(MessagesConstants.ERR_DELETE));
        }

        /// <summary>
        /// DeviceDto verilerine gore sorgualama yapar
        /// </summary>
        /// <param name="deviceDto"></param>
        /// <returns></returns>
        [Transactional(false)]
        [Authorized("UNAUTHORIZED")]
        public ServiceResponse<IEnumerable<DeviceDto>> ListAllDevice()
        {
            var Rm = new RepositoryManager(ServiceContext);
            IEnumerable<Device> list = Rm.DeviceRepository.ListAll();
            IEnumerable<DeviceDto> restulList = list.Select(entity => entity.CopyTo(new DeviceDto())).ToList();

            if (list == null || !list.Any())
                throw new ServiceException(ExceptionType.Warning, Translate(MessagesConstants.WRN_RECORD_NOT_FOUND));
            return new ServiceResponse<IEnumerable<DeviceDto>>(restulList);
        }



        #endregion Device

        #region DeviceProperty

        /// <summary>
        /// DeviceProperty kaydet
        /// </summary>
        /// <param name="DevicePropertyDto"></param>
        /// <returns></returns>
        [Transactional(true)]
        [Authorized("DEVICEPROPERTY_INSERT")]
        public ServiceResponse<long> InsertDeviceProperty(DevicePropertyDto devicePropertyDto)
        {
            if (devicePropertyDto == null) throw new ServiceException(Translate(MessagesConstants.ERR_DATA_NOT_FOUND_TO_SAVE));

            DeviceProperty entity = new DeviceProperty();
            var Rm = new RepositoryManager(ServiceContext);
            long id = Rm.DevicePropertyRepository.Insert(entity.CopyFrom(devicePropertyDto));
            if (id > 0)
                return new ServiceResponse<long>(id, Translate(MessagesConstants.SCC_DATA_INSERTED));
            throw new ServiceException(Translate(MessagesConstants.ERR_INSERT));
        }

        /// <summary>
        /// Id si verilen Device getirir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Transactional(false)]
        [Authorized("DEVICEPROPERTY_GET")]
        public ServiceResponse<DevicePropertyDto> GetDeviceProperty(long id)
        {
            var Rm = new RepositoryManager(ServiceContext);
            DeviceProperty entity = Rm.DevicePropertyRepository.Get(id);
            if (entity == null)
                throw new ServiceException(ExceptionType.Warning, Translate(MessagesConstants.WRN_RECORD_NOT_FOUND));
            return new ServiceResponse<DevicePropertyDto>(entity.CopyTo(new DevicePropertyDto()));
        }

        /// <summary>
        /// verilen DevicePropertyDto yu gunceller
        /// </summary>
        /// <param name="devicePropertyDto"></param>
        /// <returns></returns>
        [Transactional(true)]
        [Authorized("DEVICEPROPERTY_UPDATE")]
        public ServiceResponse<int> UpdateDeviceProperty(DevicePropertyDto devicePropertyDto)
        {
            if (devicePropertyDto == null) throw new ServiceException(Translate(MessagesConstants.ERR_DATA_NOT_FOUND_TO_SAVE));

            DeviceProperty entity = new DeviceProperty();
            var Rm = new RepositoryManager(ServiceContext);
            int rowsAffected = Rm.DevicePropertyRepository.Update(entity.CopyFrom(devicePropertyDto));

            if (rowsAffected > 0)
                return new ServiceResponse<int>(rowsAffected, Translate(MessagesConstants.SCC_DATA_UPDATED));
            throw new ServiceException(Translate(MessagesConstants.ERR_UPDATE));
        }

        /// <summary>
        /// Sistemden language siler
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Transactional(true)]
        [Authorized("DEVICEPROPERTY_DELETE")]
        public ServiceResponse<int> DeleteDeviceProperty(long id)
        {
            var Rm = new RepositoryManager(ServiceContext);
            int rowsAffected = Rm.DevicePropertyRepository.Delete(id);
            if (rowsAffected > 0)
                return new ServiceResponse<int>(rowsAffected, Translate(MessagesConstants.SCC_DATA_DELETED));
            throw new ServiceException(Translate(MessagesConstants.ERR_DELETE));
        }

        /// <summary>
        /// DevicePropertyDto verilerine gore sorgualama yapar
        /// </summary>
        /// <param name="devicePropertyDto"></param>
        /// <returns></returns>
        [Transactional(false)]
        [Authorized("DEVICEPROPERTY_LIST")]
        public ServiceResponse<IEnumerable<DevicePropertyDto>> ListDeviceProperty(DevicePropertyDto devicePropertyDto, PagingDto pagingDto)
        {
            var Rm = new RepositoryManager(ServiceContext);
            IEnumerable<DeviceProperty> list = Rm.DevicePropertyRepository.List(devicePropertyDto, ref pagingDto);
            IEnumerable<DevicePropertyDto> restulList = list.Select(entity => entity.CopyTo(new DevicePropertyDto())).ToList();

            if (list == null || !list.Any())
                throw new ServiceException(ExceptionType.Warning, Translate(MessagesConstants.WRN_RECORD_NOT_FOUND));
            return new ServiceResponse<IEnumerable<DevicePropertyDto>>(restulList, pagingDto.count);
        }

        /// <summary>
        /// Sistemden language siler
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Transactional(true)]
        [Authorized("DEVICEPROPERTY_DELETE_HARD")]
        public ServiceResponse<int> DeleteHardDeviceProperty(long id)
        {
            var Rm = new RepositoryManager(ServiceContext);
            int rowsAffected = Rm.DevicePropertyRepository.DeleteHard(id);
            if (rowsAffected > 0)
                return new ServiceResponse<int>(rowsAffected, Translate(MessagesConstants.SCC_DATA_DELETED));
            throw new ServiceException(Translate(MessagesConstants.ERR_DELETE));
        }

        /// <summary>
        /// DevicePropertyDto verilerine gore sorgualama yapar
        /// </summary>
        /// <param name="devicePropertyDto"></param>
        /// <returns></returns>
        [Transactional(false)]
        [Authorized("UNAUTHORIZED")]
        public ServiceResponse<IEnumerable<DevicePropertyDto>> ListAllDeviceProperty()
        {
            var Rm = new RepositoryManager(ServiceContext);
            IEnumerable<DeviceProperty> list = Rm.DevicePropertyRepository.ListAll();
            IEnumerable<DevicePropertyDto> restulList = list.Select(entity => entity.CopyTo(new DevicePropertyDto())).ToList();

            if (list == null || !list.Any())
                throw new ServiceException(ExceptionType.Warning, Translate(MessagesConstants.WRN_RECORD_NOT_FOUND));
            return new ServiceResponse<IEnumerable<DevicePropertyDto>>(restulList);
        }

        #endregion DeviceProperty

    }
}