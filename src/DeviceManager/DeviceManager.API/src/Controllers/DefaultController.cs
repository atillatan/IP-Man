using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.WebEncoders;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Text;
using System.Linq;

using Core.Framework;
using Core.Framework.Service;
using DeviceManager.API.Controllers;
using Core.Framework.Util;

namespace DeviceManager.API.Controllers
{
    public class DefaultController : BaseAPIController<DefaultController>
    {
        public IActionResult Index()
        {
            string requestedUrl = Request.Path.Value.ToLower();

            if (requestedUrl.Contains("/js/json/") && requestedUrl.EndsWith(".js"))
                return HandleJsRequest(requestedUrl);

            Response.StatusCode = 404;
            return Content("404");
        }
        private IActionResult HandleJsRequest(string requestedUrl)
        {
            Response.ContentType = "application/javascript";
            Response.StatusCode = 404;
            ContentResult contentResult = Content("404");

            string fileName = requestedUrl.Substring(requestedUrl.LastIndexOf("/") + 1).Replace(".js", "").ToUpper(new System.Globalization.CultureInfo("en-US"));

            //Handle language files
            if (requestedUrl.Contains("/js/json/languages/") && requestedUrl.EndsWith(".js"))
            {
                StringBuilder sb = new StringBuilder();
                var Cache = Application.Current.GetService<LanguageService>().Cache(fileName);

                foreach (string key in Cache.Keys)
                {
                    string val = (string)Cache[key];
                    sb.Append("\"").Append(key).Append("\":\"").Append(val).Append("\",");
                }
                Response.StatusCode = 200;
                string result = sb.ToString();
                if (result.Length == 0) return Content("404");
                return Content("{" + result.Substring(0, result.Length - 1) + "}", "text/javascript");
            }

            //Handle other js files
            switch (fileName)
            {
                case "CONFIG":

                    var config = new
                    {
                        Name = ConfigManager.Get<string>("app.name"),
                        AssetsUrl = ConfigManager.Get<string>("AssetsUrl"),
                        DefaultLanguage = ConfigManager.Get<string>("DefaultLanguage"),
                        DefaultPagingSize = ConfigManager.Get<string>("DefaultPagingSize"),
                        DefaultBrowserTitle = ConfigManager.Get<string>("DefaultBrowserTitle"),
                        DefaultAPIAddress = ConfigManager.Get<string>("DefaultAPIAddress"),
                        SSOAddress = ConfigManager.Get<string>("SSOAddress"),
                        SSOClientId = ConfigManager.Get<string>("SSOClientId"),
                        AllowedMaxExportSize = ConfigManager.Get<string>("AllowedMaxExportSize"),
                        FileUploadPath = ConfigManager.Get<string>("FileUploadPath")
                    };
                    Response.StatusCode = 200;
                    contentResult = Content($"{JsonConvert.SerializeObject(config)}");
                    break;

                case "CONSTANTS":
                    //..
                    break;

                default: // Code result

                    var cache = CacheUtil.Cache(fileName);

                    if (cache == null) break;

                    IList<dynamic> list = new List<dynamic>();

                    foreach (string key in cache.Keys)
                    {
                        dynamic cacheItem = cache[key];
                        if (cacheItem?.AktifMi == false) continue;
                        list.Add(cacheItem);
                    }

                    IList<dynamic> sortedList = list.OrderBy(a => a.Order).ToList<dynamic>();
                    string result = $"var {fileName}={JsonConvert.SerializeObject(sortedList)}";
                    Response.StatusCode = 200;
                    contentResult = Content(result, "text/javascript");
                    break;
            }

            return contentResult;
        }
    }
}