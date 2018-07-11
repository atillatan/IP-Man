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



namespace DeviceManager.Service.Tests
{
    public class SystemTest : TestBase
    {
        [Fact]
        public void Settings_CRUD()
        {
            var proxy = new ProxyService<SystemService>(new SystemService(new ServiceContext()));
            
            //arrange
            var dto = testUtil.SettingsDto();

            //Create
            var rpInsert = proxy.RunAsync(x => x.InsertSettings(dto));
            rpInsert.Wait();
            Assert.True(rpInsert.Result.Data > 0);

            //Read
            long identity = rpInsert.Result.Data;
            var rpGet = proxy.RunAsync(x => x.GetSettings(identity));
            rpGet.Wait();
            Assert.True(rpInsert.Result.Data == rpGet.Result.Data.Id);

            //Update
            var tmpDto = rpGet.Result.Data;
            tmpDto.Key = "Name updated!";
            var rpUpdate = proxy.RunAsync(x => x.UpdateSettings(tmpDto));
            rpUpdate.Wait();
            var rpUpdateGet = proxy.RunAsync(x => x.GetSettings(identity));
            rpUpdateGet.Wait();
            Assert.Equal(rpUpdateGet.Result.Data.Key, tmpDto.Key);

            //Delete
            var rpDelete = proxy.RunAsync(x => x.DeleteSettings(identity));
            rpDelete.Wait();
            var rpDeleteGet = proxy.RunAsync(x => x.GetSettings(identity));
            rpDeleteGet.Wait();
            Assert.True(rpDeleteGet.Result.Data == null);

            //List
            var queryDto = new SettingsDto { };
            var pagingDto = new PagingDto { pageSize = 3, pageNumber = 1 };
            var rpList = proxy.RunAsync(x => x.ListSettings(queryDto, pagingDto));//List
            rpList.Wait();
            Assert.True(rpList?.Result.Data != null && rpList.Result.Data.Any());

            //assert

            //cleaup
            var rpHardDelete = proxy.RunAsync(x => x.DeleteHardSettings(identity)); 
        }


        [Fact]
        public void LanguageCRUD()
        {
            var proxy = new ProxyService<SystemService>(new SystemService(new ServiceContext()));

            //arrange
            var dto = testUtil.LanguageDto();

            //Create
            var rpInsert = proxy.RunAsync(x => x.InsertLanguage(dto));
            rpInsert.Wait();
            Assert.True(rpInsert.Result.Data > 0);

            //Read
            long identity = rpInsert.Result.Data;
            var rpGet = proxy.RunAsync(x => x.GetLanguage(identity));
            rpGet.Wait();
            Assert.True(identity == rpGet.Result.Data.Id);

            //Update
            var tmpDto = rpGet.Result.Data;
            tmpDto.Key = "Name updated!";
            var rpUpdate = proxy.RunAsync(x => x.UpdateLanguage(tmpDto));
            rpUpdate.Wait();
            var rpUpdateGet = proxy.RunAsync(x => x.GetLanguage(identity));
            rpUpdateGet.Wait();
            Assert.Equal(rpUpdateGet.Result.Data.Key, tmpDto.Key);

            //Delete
            var rpDelete = proxy.RunAsync(x => x.DeleteLanguage(identity));
            rpDelete.Wait();
            var rpDeleteGet = proxy.RunAsync(x => x.GetLanguage(identity));
            rpDeleteGet.Wait();
            Assert.True(rpDeleteGet.Result.Data == null);

            //List
            var queryDto = new LanguageDto();
            var pagingDto = new PagingDto { pageSize = 3, pageNumber = 1 };
            var rpList = proxy.RunAsync(x => x.ListLanguage(queryDto, pagingDto));
            rpList.Wait();
            Assert.True(rpList != null && rpList.Result.Data != null && rpList.Result.Data.Any());

            //assert

            //cleaup
            var rpHardDelete = proxy.RunAsync(x => x.DeleteHardLanguage(identity)); 
        }







    }
}