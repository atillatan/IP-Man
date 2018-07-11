/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by:   Atilla Tanrikulu 
 * @Last Modified time: 2018-04-16 10:10:45 
 */
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Framework.Service;
using Core.Framework.Repository.DTO;
using DeviceManager.DTO.System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using DeviceManager.DTO;

namespace DeviceManager.API.Controllers
{
    [Route("api/[controller]/[action]")]
    // [Authorize]
    public class DeviceController : BaseAPIController<DeviceController>
    {
        #region NetworkService

        // GET api/device/discovernetwork
        [HttpGet()]
        public async Task<ServiceResponse<Dictionary<string, object>>> DiscoverNetwork() => await Sm.NetworkService.RunAsync(x => x.DiscoverNetwork());

        [HttpGet()]
        public async Task<ServiceResponse<Dictionary<string, object>>> GetDevicesFromCache() => await Sm.NetworkService.RunAsync(x => x.GetDevicesFromCache());

        #endregion NetworkService

        #region DeviceService.Property

        // POST api/device/postproperty
        [HttpPost]
        public async Task<ServiceResponse<long>> PostProperty([FromBody] PropertyDto propertyDto) => await Sm.DeviceService.RunAsync(x => x.InsertProperty(propertyDto));

        // GET api/device/getproperty/5
        [HttpGet("{id}")]
        public async Task<ServiceResponse<PropertyDto>> GetProperty(int id) => await Sm.DeviceService.RunAsync(x => x.GetProperty(id));

        // PUT api/device/putproperty/id
        [HttpPut("{id:int?}")]
        public async Task<ServiceResponse<int>> PutProperty(int id, [FromBody] PropertyDto propertyDto) => await Sm.DeviceService.RunAsync(x => x.UpdateProperty(propertyDto));

        // DELETE api/device/deleteproperty/id
        [HttpDelete("{id}")]
        public async Task<ServiceResponse<int>> DeleteProperty(int id) => await Sm.DeviceService.RunAsync(x => x.DeleteProperty(id));

        // GET api/device/listproperty
        [HttpPost]
        public async Task<ServiceResponse<IEnumerable<PropertyDto>>> ListProperty([FromBody]IDictionary<string, object> data)
        {
            if (data == null) throw new System.ArgumentNullException(nameof(data));
            PropertyDto searchDto = JsonConvert.DeserializeObject<PropertyDto>(data["searchDto"].ToString());
            PagingDto pagingDto = JsonConvert.DeserializeObject<PagingDto>(data["pagingDto"].ToString());

            return await Sm.DeviceService.RunAsync(x => x.ListProperty(searchDto, pagingDto));
        }

        #endregion DeviceService.Property

        #region DeviceService.Model

        // POST api/device/postmodel
        [HttpPost]
        public async Task<ServiceResponse<long>> PostModel([FromBody] ModelDto modelDto) => await Sm.DeviceService.RunAsync(x => x.InsertModel(modelDto));

        // GET api/device/getmodel/5
        [HttpGet("{id}")]
        public async Task<ServiceResponse<ModelDto>> GetModel(int id) => await Sm.DeviceService.RunAsync(x => x.GetModel(id));

        // PUT api/device/putmodel/id
        [HttpPut("{id:int?}")]
        public async Task<ServiceResponse<int>> PutModel(int id, [FromBody] ModelDto modelDto) => await Sm.DeviceService.RunAsync(x => x.UpdateModel(modelDto));

        // DELETE api/device/deletemodel/id
        [HttpDelete("{id}")]
        public async Task<ServiceResponse<int>> DeleteModel(int id) => await Sm.DeviceService.RunAsync(x => x.DeleteModel(id));

        // GET api/device/listmodel
        [HttpPost]
        public async Task<ServiceResponse<IEnumerable<ModelDto>>> ListModel([FromBody]IDictionary<string, object> data)
        {
            if (data == null) throw new System.ArgumentNullException(nameof(data));
            ModelDto searchDto = JsonConvert.DeserializeObject<ModelDto>(data["searchDto"].ToString());
            PagingDto pagingDto = JsonConvert.DeserializeObject<PagingDto>(data["pagingDto"].ToString());

            return await Sm.DeviceService.RunAsync(x => x.ListModel(searchDto, pagingDto));
        }

        #endregion DeviceService.Model

        #region DeviceService.Template

        // POST api/device/posttemplate
        [HttpPost]
        public async Task<ServiceResponse<long>> PostTemplate([FromBody] TemplateDto templateDto) => await Sm.DeviceService.RunAsync(x => x.InsertTemplate(templateDto));

        // GET api/device/gettemplate/5
        [HttpGet("{id}")]
        public async Task<ServiceResponse<TemplateDto>> GetTemplate(int id) => await Sm.DeviceService.RunAsync(x => x.GetTemplate(id));

        // PUT api/device/puttemplate/id
        [HttpPut("{id:int?}")]
        public async Task<ServiceResponse<int>> PutTemplate(int id, [FromBody] TemplateDto templateDto) => await Sm.DeviceService.RunAsync(x => x.UpdateTemplate(templateDto));

        // DELETE api/device/deletetemplate/id
        [HttpDelete("{id}")]
        public async Task<ServiceResponse<int>> DeleteTemplate(int id) => await Sm.DeviceService.RunAsync(x => x.DeleteTemplate(id));

        // GET api/device/listtemplate
        [HttpPost]
        public async Task<ServiceResponse<IEnumerable<TemplateDto>>> ListTemplate([FromBody]IDictionary<string, object> data)
        {
            if (data == null) throw new System.ArgumentNullException(nameof(data));
            TemplateDto searchDto = JsonConvert.DeserializeObject<TemplateDto>(data["searchDto"].ToString());
            PagingDto pagingDto = JsonConvert.DeserializeObject<PagingDto>(data["pagingDto"].ToString());

            return await Sm.DeviceService.RunAsync(x => x.ListTemplate(searchDto, pagingDto));
        }

        #endregion DeviceService.Template

        #region DeviceService.TemplateProperty

        // POST api/device/posttemplateproperty
        [HttpPost]
        public async Task<ServiceResponse<long>> PostTemplateProperty([FromBody] TemplatePropertyDto templatepropertyDto) => await Sm.DeviceService.RunAsync(x => x.InsertTemplateProperty(templatepropertyDto));

        // GET api/device/gettemplateproperty/5
        [HttpGet("{id}")]
        public async Task<ServiceResponse<TemplatePropertyDto>> GetTemplateProperty(int id) => await Sm.DeviceService.RunAsync(x => x.GetTemplateProperty(id));

        // PUT api/device/puttemplateproperty/id
        [HttpPut("{id:int?}")]
        public async Task<ServiceResponse<int>> PutTemplateProperty(int id, [FromBody] TemplatePropertyDto templatepropertyDto) => await Sm.DeviceService.RunAsync(x => x.UpdateTemplateProperty(templatepropertyDto));

        // DELETE api/device/deletetemplateproperty/id
        [HttpDelete("{id}")]
        public async Task<ServiceResponse<int>> DeleteTemplateProperty(int id) => await Sm.DeviceService.RunAsync(x => x.DeleteTemplateProperty(id));

        // GET api/device/listtemplateproperty
        [HttpPost]
        public async Task<ServiceResponse<IEnumerable<TemplatePropertyDto>>> ListTemplateProperty([FromBody]IDictionary<string, object> data)
        {
            if (data == null) throw new System.ArgumentNullException(nameof(data));
            TemplatePropertyDto searchDto = JsonConvert.DeserializeObject<TemplatePropertyDto>(data["searchDto"].ToString());
            PagingDto pagingDto = JsonConvert.DeserializeObject<PagingDto>(data["pagingDto"].ToString());

            return await Sm.DeviceService.RunAsync(x => x.ListTemplateProperty(searchDto, pagingDto));
        }

        #endregion DeviceService.TemplateProperty

        #region DeviceService.Device

        // POST api/device/postdevice
        [HttpPost]
        public async Task<ServiceResponse<long>> PostDevice([FromBody] DeviceDto deviceDto) => await Sm.DeviceService.RunAsync(x => x.InsertDevice(deviceDto));

        // GET api/device/getdevice/5
        [HttpGet("{id}")]
        public async Task<ServiceResponse<DeviceDto>> GetDevice(int id) => await Sm.DeviceService.RunAsync(x => x.GetDevice(id));

        // PUT api/device/putdevice/id
        [HttpPut("{id:int?}")]
        public async Task<ServiceResponse<int>> PutDevice(int id, [FromBody] DeviceDto deviceDto) => await Sm.DeviceService.RunAsync(x => x.UpdateDevice(deviceDto));

        // DELETE api/device/deletedevice/id
        [HttpDelete("{id}")]
        public async Task<ServiceResponse<int>> DeleteDevice(int id) => await Sm.DeviceService.RunAsync(x => x.DeleteDevice(id));

        // GET api/device/listdevice
        [HttpPost]
        public async Task<ServiceResponse<IEnumerable<DeviceDto>>> ListDevice([FromBody]IDictionary<string, object> data)
        {
            if (data == null) throw new System.ArgumentNullException(nameof(data));
            DeviceDto searchDto = JsonConvert.DeserializeObject<DeviceDto>(data["searchDto"].ToString());
            PagingDto pagingDto = JsonConvert.DeserializeObject<PagingDto>(data["pagingDto"].ToString());

            return await Sm.DeviceService.RunAsync(x => x.ListDevice(searchDto, pagingDto));
        }

        #endregion DeviceService.Device

        #region DeviceService.DeviceProperty

        // POST api/device/postdeviceproperty
        [HttpPost]
        public async Task<ServiceResponse<long>> PostDeviceProperty([FromBody] DevicePropertyDto devicepropertyDto) => await Sm.DeviceService.RunAsync(x => x.InsertDeviceProperty(devicepropertyDto));

        // GET api/device/getdeviceproperty/5
        [HttpGet("{id}")]
        public async Task<ServiceResponse<DevicePropertyDto>> GetDeviceProperty(int id) => await Sm.DeviceService.RunAsync(x => x.GetDeviceProperty(id));

        // PUT api/device/putdeviceproperty/id
        [HttpPut("{id:int?}")]
        public async Task<ServiceResponse<int>> PutDeviceProperty(int id, [FromBody] DevicePropertyDto devicepropertyDto) => await Sm.DeviceService.RunAsync(x => x.UpdateDeviceProperty(devicepropertyDto));

        // DELETE api/device/deletedeviceproperty/id
        [HttpDelete("{id}")]
        public async Task<ServiceResponse<int>> DeleteDeviceProperty(int id) => await Sm.DeviceService.RunAsync(x => x.DeleteDeviceProperty(id));

        // GET api/device/listdeviceproperty
        [HttpPost]
        public async Task<ServiceResponse<IEnumerable<DevicePropertyDto>>> ListDeviceProperty([FromBody]IDictionary<string, object> data)
        {
            if (data == null) throw new System.ArgumentNullException(nameof(data));
            DevicePropertyDto searchDto = JsonConvert.DeserializeObject<DevicePropertyDto>(data["searchDto"].ToString());
            PagingDto pagingDto = JsonConvert.DeserializeObject<PagingDto>(data["pagingDto"].ToString());

            return await Sm.DeviceService.RunAsync(x => x.ListDeviceProperty(searchDto, pagingDto));
        }

        #endregion DeviceService.DeviceProperty

    }
}