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
using DeviceManager.DTO;

namespace DeviceManager.Service.Tests
{
    public class DeviceTest : TestBase
    {

        [Fact]
        public void Model_CRUD()
        {
            var proxy = new ProxyService<DeviceService>(new DeviceService(new ServiceContext()));

            //arrange
            var dto = testUtil.ModelDto();

            //Create
            var rpInsert = proxy.RunAsync(x => x.InsertModel(dto));
            rpInsert.Wait();
            Assert.True(rpInsert.Result.Data > 0);

            //Read
            long identity = rpInsert.Result.Data;
            var rpGet = proxy.RunAsync(x => x.GetModel(identity));
            rpGet.Wait();
            Assert.True(rpInsert.Result.Data == rpGet.Result.Data.Id);

            //Update
            var tmpDto = rpGet.Result.Data;
            tmpDto.BrandName = "Name updated!";
            var rpUpdate = proxy.RunAsync(x => x.UpdateModel(tmpDto));
            rpUpdate.Wait();
            var rpUpdateGet = proxy.RunAsync(x => x.GetModel(identity));
            rpUpdateGet.Wait();
            Assert.Equal(rpUpdateGet.Result.Data.BrandName, tmpDto.BrandName);

            //Delete
            var rpDelete = proxy.RunAsync(x => x.DeleteModel(identity));
            rpDelete.Wait();
            var rpDeleteGet = proxy.RunAsync(x => x.GetModel(identity));
            rpDeleteGet.Wait();
            Assert.True(rpDeleteGet.Result.Data == null);

            //List
            var queryDto = new ModelDto { };
            var pagingDto = new PagingDto { pageSize = 30, pageNumber = 1 };
            var rpList = proxy.RunAsync(x => x.ListModel(queryDto, pagingDto));//List
            rpList.Wait();
            Assert.True(rpList?.Result.Data != null && rpList.Result.Data.Any());

            //assert

            //cleaup
            var rpHardDelete = proxy.RunAsync(x => x.DeleteHardModel(identity));
        }

        [Fact]
        public void Property_CRUD()
        {
            var proxy = new ProxyService<DeviceService>(new DeviceService(new ServiceContext()));

            //arrange
            var dto = testUtil.PropertyDto();

            //Create
            var rpInsert = proxy.RunAsync(x => x.InsertProperty(dto));
            rpInsert.Wait();
            Assert.True(rpInsert.Result.Data > 0);

            //Read
            long identity = rpInsert.Result.Data;
            var rpGet = proxy.RunAsync(x => x.GetProperty(identity));
            rpGet.Wait();
            Assert.True(rpInsert.Result.Data == rpGet.Result.Data.Id);

            //Update
            var tmpDto = rpGet.Result.Data;
            tmpDto.Name = "Name updated!";
            var rpUpdate = proxy.RunAsync(x => x.UpdateProperty(tmpDto));
            rpUpdate.Wait();
            var rpUpdateGet = proxy.RunAsync(x => x.GetProperty(identity));
            rpUpdateGet.Wait();
            Assert.Equal(rpUpdateGet.Result.Data.Name, tmpDto.Name);

            //Delete
            var rpDelete = proxy.RunAsync(x => x.DeleteProperty(identity));
            rpDelete.Wait();
            var rpDeleteGet = proxy.RunAsync(x => x.GetProperty(identity));
            rpDeleteGet.Wait();
            Assert.True(rpDeleteGet.Result.Data == null);

            //List
            var queryDto = new PropertyDto { };
            var pagingDto = new PagingDto { pageSize = 3, pageNumber = 1 };
            var rpList = proxy.RunAsync(x => x.ListProperty(queryDto, pagingDto));//List
            rpList.Wait();
            Assert.True(rpList?.Result.Data != null && rpList.Result.Data.Any());

            //assert

            //cleaup
            var rpHardDelete = proxy.RunAsync(x => x.DeleteHardProperty(identity));
        }

        [Fact]
        public void Template_CRUD()
        {
            var proxy = new ProxyService<DeviceService>(new DeviceService(new ServiceContext()));

            //arrange
            var dto = testUtil.TemplateDto();

            //Create
            var rpInsert = proxy.RunAsync(x => x.InsertTemplate(dto));
            rpInsert.Wait();
            Assert.True(rpInsert.Result.Data > 0);

            //Read
            long identity = rpInsert.Result.Data;
            var rpGet = proxy.RunAsync(x => x.GetTemplate(identity));
            rpGet.Wait();
            Assert.True(rpInsert.Result.Data == rpGet.Result.Data.Id);

            //Update
            var tmpDto = rpGet.Result.Data;
            tmpDto.Name = "Name updated!";
            var rpUpdate = proxy.RunAsync(x => x.UpdateTemplate(tmpDto));
            rpUpdate.Wait();
            var rpUpdateGet = proxy.RunAsync(x => x.GetTemplate(identity));
            rpUpdateGet.Wait();
            Assert.Equal(rpUpdateGet.Result.Data.Name, tmpDto.Name);

            //Delete
            var rpDelete = proxy.RunAsync(x => x.DeleteTemplate(identity));
            rpDelete.Wait();
            var rpDeleteGet = proxy.RunAsync(x => x.GetTemplate(identity));
            rpDeleteGet.Wait();
            Assert.True(rpDeleteGet.Result.Data == null);

            //List
            var queryDto = new TemplateDto { };
            var pagingDto = new PagingDto { pageSize = 3, pageNumber = 1 };
            var rpList = proxy.RunAsync(x => x.ListTemplate(queryDto, pagingDto));//List
            rpList.Wait();
            Assert.True(rpList?.Result.Data != null && rpList.Result.Data.Any());

            //assert

            //cleaup
            var rpHardDelete = proxy.RunAsync(x => x.DeleteHardTemplate(identity));
        }

        [Fact]
        public void TemplateProperty_CRUD()
        {
            var proxy = new ProxyService<DeviceService>(new DeviceService(new ServiceContext()));

            //arrange
            var dto = testUtil.TemplatePropertyDto();

            //Create
            var rpInsert = proxy.RunAsync(x => x.InsertTemplateProperty(dto));
            rpInsert.Wait();
            Assert.True(rpInsert.Result.Data > 0);

            //Read
            long identity = rpInsert.Result.Data;
            var rpGet = proxy.RunAsync(x => x.GetTemplateProperty(identity));
            rpGet.Wait();
            Assert.True(rpInsert.Result.Data == rpGet.Result.Data.Id);

            //Update
            var tmpDto = rpGet.Result.Data;
            tmpDto.DefaultValue = "Name updated!";
            var rpUpdate = proxy.RunAsync(x => x.UpdateTemplateProperty(tmpDto));
            rpUpdate.Wait();
            var rpUpdateGet = proxy.RunAsync(x => x.GetTemplateProperty(identity));
            rpUpdateGet.Wait();
            Assert.Equal(rpUpdateGet.Result.Data.DefaultValue, tmpDto.DefaultValue);

            //Delete
            var rpDelete = proxy.RunAsync(x => x.DeleteTemplateProperty(identity));
            rpDelete.Wait();
            var rpDeleteGet = proxy.RunAsync(x => x.GetTemplateProperty(identity));
            rpDeleteGet.Wait();
            Assert.True(rpDeleteGet.Result.Data == null);

            //List
            var queryDto = new TemplatePropertyDto { };
            var pagingDto = new PagingDto { pageSize = 3, pageNumber = 1 };
            var rpList = proxy.RunAsync(x => x.ListTemplateProperty(queryDto, pagingDto));//List
            rpList.Wait();
            Assert.True(rpList?.Result.Data != null && rpList.Result.Data.Any());

            //assert

            //cleaup
            var rpHardDelete = proxy.RunAsync(x => x.DeleteHardTemplateProperty(identity));
        }

        [Fact]
        public void Device_CRUD()
        {
            var proxy = new ProxyService<DeviceService>(new DeviceService(new ServiceContext()));

            //arrange
            var dto = testUtil.DeviceDto();

            //Create
            var rpInsert = proxy.RunAsync(x => x.InsertDevice(dto));
            rpInsert.Wait();
            Assert.True(rpInsert.Result.Data > 0);

            //Read
            long identity = rpInsert.Result.Data;
            var rpGet = proxy.RunAsync(x => x.GetDevice(identity));
            rpGet.Wait();
            Assert.True(rpInsert.Result.Data == rpGet.Result.Data.Id);

            //Update
            var tmpDto = rpGet.Result.Data;
            tmpDto.DeviceCode = "Name updated!";
            var rpUpdate = proxy.RunAsync(x => x.UpdateDevice(tmpDto));
            rpUpdate.Wait();
            var rpUpdateGet = proxy.RunAsync(x => x.GetDevice(identity));
            rpUpdateGet.Wait();
            Assert.Equal(rpUpdateGet.Result.Data.DeviceCode, tmpDto.DeviceCode);

            //Delete
            var rpDelete = proxy.RunAsync(x => x.DeleteDevice(identity));
            rpDelete.Wait();
            var rpDeleteGet = proxy.RunAsync(x => x.GetDevice(identity));
            rpDeleteGet.Wait();
            Assert.True(rpDeleteGet.Result.Data == null);

            //List
            var queryDto = new DeviceDto { LastAccessDate = null };
            var pagingDto = new PagingDto { pageSize = 3, pageNumber = 1 };
            var rpList = proxy.RunAsync(x => x.ListDevice(queryDto, pagingDto));//List
            rpList.Wait();
            Assert.True(rpList?.Result.Data != null && rpList.Result.Data.Any());
            //assert

            //cleaup
            var rpHardDelete = proxy.RunAsync(x => x.DeleteHardDevice(identity));
        }

        [Fact]
        public void DeviceProperty_CRUD()
        {
            var proxy = new ProxyService<DeviceService>(new DeviceService(new ServiceContext()));

            //arrange
            var dto = testUtil.DevicePropertyDto();

            //Create
            var rpInsert = proxy.RunAsync(x => x.InsertDeviceProperty(dto));
            rpInsert.Wait();
            Assert.True(rpInsert.Result.Data > 0);

            //Read
            long identity = rpInsert.Result.Data;
            var rpGet = proxy.RunAsync(x => x.GetDeviceProperty(identity));
            rpGet.Wait();
            Assert.True(rpInsert.Result.Data == rpGet.Result.Data.Id);

            //Update
            var tmpDto = rpGet.Result.Data;
            tmpDto.PropertyValue = "Name updated!";
            var rpUpdate = proxy.RunAsync(x => x.UpdateDeviceProperty(tmpDto));
            rpUpdate.Wait();
            var rpUpdateGet = proxy.RunAsync(x => x.GetDeviceProperty(identity));
            rpUpdateGet.Wait();
            Assert.Equal(rpUpdateGet.Result.Data.PropertyValue, tmpDto.PropertyValue);

            //Delete
            var rpDelete = proxy.RunAsync(x => x.DeleteDeviceProperty(identity));
            rpDelete.Wait();
            var rpDeleteGet = proxy.RunAsync(x => x.GetDeviceProperty(identity));
            rpDeleteGet.Wait();
            Assert.True(rpDeleteGet.Result.Data == null);

            //List
            var queryDto = new DevicePropertyDto { };
            var pagingDto = new PagingDto { pageSize = 3, pageNumber = 1 };
            var rpList = proxy.RunAsync(x => x.ListDeviceProperty(queryDto, pagingDto));//List
            rpList.Wait();
            Assert.True(rpList?.Result.Data != null && rpList.Result.Data.Any());

            //assert

            //cleaup
            var rpHardDelete = proxy.RunAsync(x => x.DeleteHardDeviceProperty(identity));
        }

    }
}