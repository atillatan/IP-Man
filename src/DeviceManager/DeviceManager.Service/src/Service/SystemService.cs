/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by: Atilla Tanrikulu
 * @Last Modified time: 2018-05-02 18:21:18
 */
using System.Collections.Generic;
using System.Linq;
using Core.Framework.Service;
using Core.Framework.Repository.DTO;
using Core.Framework.Middleware;
using Core.Framework;
using DeviceManager.Service.Repository;
using DeviceManager.Service.Entity.System;
using DeviceManager.DTO.System;
using log4net;
using System.Net.NetworkInformation;
using System;
using System.Net;

namespace DeviceManager.Service.Service
{
    [Authorized]
    public class SystemService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SystemService));

        public ServiceContext ServiceContext { get; set; }
        public SystemService(ServiceContext serviceContext)
        {
            this.ServiceContext = serviceContext;
        }

        public LanguageService LanguageService = Application.Current.GetService<LanguageService>();
        public string Translate(string key) => LanguageService.Translate(key, ServiceContext.UserInfo.Language);

        #region Settings
        /// <summary>
        /// veriyi sisteme kaydeder
        /// </summary>
        /// <param name="settingsDto">Kaydedilecek SettingsDto nesnesi</param>
        /// <returns></returns>
        [Transactional(true)]
        [Authorized("SETTINGS_INSERT")]
        public ServiceResponse<long> InsertSettings(SettingsDto settingsDto)
        {
            if (settingsDto == null) throw new ServiceException(Translate(MessagesConstants.ERR_DATA_NOT_FOUND_TO_SAVE));

            Settings settingsEntity = new Settings();
            var Rm = new RepositoryManager(ServiceContext);
            long id = Rm.SettingsRepository.Insert(settingsEntity.CopyFrom(settingsDto));

            if (id > 0)
                return new ServiceResponse<long>(id, Translate(MessagesConstants.SCC_DATA_INSERTED));
            throw new ServiceException(Translate(MessagesConstants.ERR_INSERT));
        }

        /// <summary>
        /// Tekil bir kayit bilgisini veri tabanindan getirir.
        /// </summary>
        /// <param name="id">Getirilecek olak settingsnin ID degeri</param>
        /// <returns></returns>
        [Transactional(false)]
        [Authorized("SETTINGS_GET")]
        public ServiceResponse<SettingsDto> GetSettings(long id)
        {
            var Rm = new RepositoryManager(ServiceContext);
            Settings settingsEntity = Rm.SettingsRepository.Get(id);
            if (settingsEntity == null)
                throw new ServiceException(Translate(MessagesConstants.WRN_RECORD_NOT_FOUND));
            return new ServiceResponse<SettingsDto>(settingsEntity.CopyTo(new SettingsDto()));
        }

        /// <summary>
        /// Kayit bilgilerini gunceller
        /// </summary>
        /// <param name="settingsDto">Guncellencek olan SettingsDto nesnesi</param>
        /// <returns></returns>
        [Transactional(true)]
        [Authorized("SETTINGS_UPDATE")]
        public ServiceResponse<int> UpdateSettings(SettingsDto settingsDto)
        {
            if (settingsDto == null) throw new ServiceException(Translate(MessagesConstants.ERR_DATA_NOT_FOUND_TO_SAVE));
            Settings settingsEntity = new Settings();
            var Rm = new RepositoryManager(ServiceContext);
            int rowsAffected = Rm.SettingsRepository.Update(settingsEntity.CopyFrom(settingsDto));
            if (rowsAffected > 0)
                return new ServiceResponse<int>(rowsAffected, Translate(MessagesConstants.SCC_DATA_UPDATED));
            throw new ServiceException(Translate(MessagesConstants.ERR_UPDATE));
        }

        /// <summary>
        /// Kayit siler
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Transactional(true)]
        [Authorized("SETTINGS_DELETE")]
        public ServiceResponse<int> DeleteSettings(long id)
        {
            var Rm = new RepositoryManager(ServiceContext);
            int rowsAffected = Rm.SettingsRepository.Delete(id);
            if (rowsAffected > 0)
                return new ServiceResponse<int>(rowsAffected, Translate(MessagesConstants.SCC_DATA_DELETED));
            throw new ServiceException(Translate(MessagesConstants.ERR_DELETE));
        }

        /// <summary>
        /// Kayitlar sayfali olarak sorgulanir. pagingDto parametresinde, sayfalama degeri, maksimum degerden kucuk olmalidir.
        /// </summary>
        /// <param name="settingsDto">Listelenecek olan verinin arama kriterleri</param>
        /// <param name="pagingDto">Sayfalama isleminde kullanilacak parametreler.</param>
        /// <returns></returns>
        [Transactional(false)]
        [Authorized("SETTINGS_LIST")]
        public ServiceResponse<IEnumerable<SettingsDto>> ListSettings(SettingsDto settingsDto, PagingDto pagingDto)
        {
            var Rm = new RepositoryManager(ServiceContext);
            IEnumerable<Settings> list = Rm.SettingsRepository.List(settingsDto, ref pagingDto);

            IEnumerable<SettingsDto> restulList = list.Select(entity => entity.CopyTo(new SettingsDto())).ToList();
            if (list == null || !list.Any())
                throw new ServiceException(ExceptionType.Warning, Translate(MessagesConstants.WRN_RECORD_NOT_FOUND));
            return new ServiceResponse<IEnumerable<SettingsDto>>(restulList, pagingDto.count);
        }

        /// <summary>
        /// Method sistem icinden kullanilacaktir, API yada WCF ile disari acilmamalidir.
        /// Method cagrilirken yetki kontrolu yapilmayacaktir.
        /// </summary>
        /// <returns></returns>
        [Transactional(true)]
        [Authorized("SETTINGS_DELETE_HARD")]
        public ServiceResponse<int> DeleteHardSettings(long id)
        {
            var Rm = new RepositoryManager(ServiceContext);
            int rowsAffected = Rm.SettingsRepository.DeleteHard(id);
            if (rowsAffected > 0)
                return new ServiceResponse<int>(rowsAffected, Translate(MessagesConstants.SCC_DATA_DELETED));
            throw new ServiceException(Translate(MessagesConstants.ERR_DELETE));
        }

        /// <summary>
        /// Kayitlar sayfali olarak sorgulanir. pagingDto parametresinde, sayfalama degeri, maksimum degerden kuzuk olmalidir.
        /// </summary>
        /// <param name="settingsDto">Listelenecek olan verinin arama kriterleri</param>
        /// <param name="pagingDto">Sayfalama isleminde kullanilacak parametreler.</param>
        /// <returns></returns>
        [Transactional(false)]
        [Authorized("SETTINGS_LIST")]
        public ServiceResponse<IEnumerable<SettingsDto>> ListSettingsInternal()
        {
            var Rm = new RepositoryManager(ServiceContext);
            IEnumerable<Settings> list = Rm.SettingsRepository.ListAll();
            IEnumerable<SettingsDto> restulList = list.Select(entity => entity.CopyTo(new SettingsDto())).ToList();

            if (list == null || !list.Any())
                throw new ServiceException(Translate(MessagesConstants.WRN_RECORD_NOT_FOUND));
            return new ServiceResponse<IEnumerable<SettingsDto>>(restulList);
        }


        #endregion Settings

        #region Language

        /// <summary>
        /// Language kaydet
        /// </summary>
        /// <param name="languageDto"></param>
        /// <returns></returns>
        [Transactional(true)]
        [Authorized("LANGUAGE_INSERT")]
        public ServiceResponse<long> InsertLanguage(LanguageDto languageDto)
        {
            if (languageDto == null) throw new ServiceException(Translate(MessagesConstants.ERR_DATA_NOT_FOUND_TO_SAVE));

            Language languageEntity = new Language();
            var Rm = new RepositoryManager(ServiceContext);
            long id = Rm.LanguageRepository.Insert(languageEntity.CopyFrom(languageDto));
            if (id > 0)
                return new ServiceResponse<long>(id, Translate(MessagesConstants.SCC_DATA_INSERTED));
            throw new ServiceException(Translate(MessagesConstants.ERR_INSERT));
        }

        /// <summary>
        /// Id si verilen language getirir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Transactional(false)]
        [Authorized("LANGUAGE_GET")]
        public ServiceResponse<LanguageDto> GetLanguage(long id)
        {
            var Rm = new RepositoryManager(ServiceContext);
            Language languageEntity = Rm.LanguageRepository.Get(id);
            if (languageEntity == null)
                throw new ServiceException(ExceptionType.Warning, Translate(MessagesConstants.WRN_RECORD_NOT_FOUND));
            return new ServiceResponse<LanguageDto>(languageEntity.CopyTo(new LanguageDto()));
        }

        /// <summary>
        /// verilen LanguageDto yu gunceller
        /// </summary>
        /// <param name="languageDto"></param>
        /// <returns></returns>
        [Transactional(true)]
        [Authorized("LANGUAGE_UPDATE")]
        public ServiceResponse<int> UpdateLanguage(LanguageDto languageDto)
        {
            if (languageDto == null) throw new ServiceException(Translate(MessagesConstants.ERR_DATA_NOT_FOUND_TO_SAVE));

            Language languageEntity = new Language();
            var Rm = new RepositoryManager(ServiceContext);
            int rowsAffected = Rm.LanguageRepository.Update(languageEntity.CopyFrom(languageDto));

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
        [Authorized("LANGUAGE_DELETE")]
        public ServiceResponse<int> DeleteLanguage(long id)
        {
            var Rm = new RepositoryManager(ServiceContext);
            int rowsAffected = Rm.LanguageRepository.Delete(id);
            if (rowsAffected > 0)
                return new ServiceResponse<int>(rowsAffected, Translate(MessagesConstants.SCC_DATA_DELETED));
            throw new ServiceException(Translate(MessagesConstants.ERR_DELETE));
        }

        /// <summary>
        /// LanguageDto verilerine gore sorgualama yapar
        /// </summary>
        /// <param name="languageDto"></param>
        /// <returns></returns>
        [Transactional(false)]
        [Authorized("LANGUAGE_LIST")]
        public ServiceResponse<IEnumerable<LanguageDto>> ListLanguage(LanguageDto languageDto, PagingDto pagingDto)
        {
            var Rm = new RepositoryManager(ServiceContext);
            IEnumerable<Language> list = Rm.LanguageRepository.List(languageDto, ref pagingDto);
            IEnumerable<LanguageDto> restulList = list.Select(entity => entity.CopyTo(new LanguageDto())).ToList();

            if (list == null || !list.Any())
                throw new ServiceException(ExceptionType.Warning, Translate(MessagesConstants.WRN_RECORD_NOT_FOUND));
            return new ServiceResponse<IEnumerable<LanguageDto>>(restulList, pagingDto.count);
        }

        /// <summary>
        /// Sistemden language siler
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Transactional(true)]
        [Authorized("LANGUAGE_DELETE_HARD")]
        public ServiceResponse<int> DeleteHardLanguage(long id)
        {
            var Rm = new RepositoryManager(ServiceContext);
            int rowsAffected = Rm.LanguageRepository.DeleteHard(id);
            if (rowsAffected > 0)
                return new ServiceResponse<int>(rowsAffected, Translate(MessagesConstants.SCC_DATA_DELETED));
            throw new ServiceException(Translate(MessagesConstants.ERR_DELETE));
        }

        /// <summary>
        /// LanguageDto verilerine gore sorgualama yapar
        /// </summary>
        /// <param name="languageDto"></param>
        /// <returns></returns>
        [Transactional(false)]
        [Authorized("UNAUTHORIZED")]
        public ServiceResponse<IEnumerable<LanguageDto>> ListAllLanguage()
        {
            var Rm = new RepositoryManager(ServiceContext);
            IEnumerable<Language> list = Rm.LanguageRepository.ListAll();
            IEnumerable<LanguageDto> restulList = list.Select(entity => entity.CopyTo(new LanguageDto())).ToList();

            if (list == null || !list.Any())
                throw new ServiceException(ExceptionType.Warning, Translate(MessagesConstants.WRN_RECORD_NOT_FOUND));
            return new ServiceResponse<IEnumerable<LanguageDto>>(restulList);
        }



        #endregion Language

    }
}