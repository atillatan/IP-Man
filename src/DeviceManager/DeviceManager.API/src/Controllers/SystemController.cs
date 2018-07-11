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

namespace DeviceManager.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class SystemController : BaseAPIController<SystemController>
    {
        #region SystemService.Language

        // POST api/example/postlanguage
        [HttpPost]
        public async Task<ServiceResponse<long>> PostLanguage([FromBody] LanguageDto languageDto) => await Sm.SystemService.RunAsync(x => x.InsertLanguage(languageDto));

        // GET api/example/getlanguage/5
        [HttpGet("{id}")]
        public async Task<ServiceResponse<LanguageDto>> GetLanguage(int id) => await Sm.SystemService.RunAsync(x => x.GetLanguage(id));

        // PUT api/example/putlanguage/id
        [HttpPut("{id:int?}")]
        public async Task<ServiceResponse<int>> PutLanguage(int id, [FromBody] LanguageDto languageDto) => await Sm.SystemService.RunAsync(x => x.UpdateLanguage(languageDto));

        // DELETE api/example/deletelanguage/id
        [HttpDelete("{id}")]
        public async Task<ServiceResponse<int>> DeleteLanguage(int id) => await Sm.SystemService.RunAsync(x => x.DeleteLanguage(id));

        // GET api/example/listlanguage
        [HttpPost]
        public async Task<ServiceResponse<IEnumerable<LanguageDto>>> ListLanguage([FromBody]IDictionary<string, object> data)
        {
            if (data == null) throw new System.ArgumentNullException(nameof(data));
            LanguageDto searchDto = JsonConvert.DeserializeObject<LanguageDto>(data["searchDto"].ToString());
            PagingDto pagingDto = JsonConvert.DeserializeObject<PagingDto>(data["pagingDto"].ToString());

            return await Sm.SystemService.RunAsync(x =>x.ListLanguage(searchDto, pagingDto));
        }

        #endregion SystemService.Language


        #region SystemService.Settings

        // POST api/example/postsettings
        [HttpPost]
        public async Task<ServiceResponse<long>> PostSettings([FromBody] SettingsDto settingsDto) => await Sm.SystemService.RunAsync(x => x.InsertSettings(settingsDto));

        // GET api/example/getsettings/5
        [HttpGet("{id}")]
        public async Task<ServiceResponse<SettingsDto>> GetSettings(int id) => await Sm.SystemService.RunAsync(x => x.GetSettings(id));

        // PUT api/example/putsettings/id
        [HttpPut("{id:int?}")]
        public async Task<ServiceResponse<int>> PutSettings(int id, [FromBody] SettingsDto settingsDto) => await Sm.SystemService.RunAsync(x => x.UpdateSettings(settingsDto));

        // DELETE api/example/deletesettings/id
        [HttpDelete("{id}")]
        public async Task<ServiceResponse<int>> DeleteSettings(int id) => await Sm.SystemService.RunAsync(x => x.DeleteSettings(id));

        // GET api/example/listsettings
        [HttpPost]
        public async Task<ServiceResponse<IEnumerable<SettingsDto>>> ListSettings([FromBody]IDictionary<string, object> data)
        {
            if (data == null) throw new System.ArgumentNullException(nameof(data));
            SettingsDto searchDto = JsonConvert.DeserializeObject<SettingsDto>(data["searchDto"].ToString());
            PagingDto pagingDto = JsonConvert.DeserializeObject<PagingDto>(data["pagingDto"].ToString());

            return await Sm.SystemService.RunAsync(x => x.ListSettings(searchDto, pagingDto));
        }

        #endregion SystemService.Settings
    }
}