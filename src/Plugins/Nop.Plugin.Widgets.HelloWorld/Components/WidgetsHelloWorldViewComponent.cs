using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.Widgets.HelloWorld.Models;
using Nop.Services.Configuration;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.HelloWorld.Components
{
    [ViewComponent(Name = "WidgetsHelloWorld")]
    public class WidgetsHelloWorldViewComponent : NopViewComponent
    {
        private readonly IStoreContext _storeContext;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;

        public WidgetsHelloWorldViewComponent(IStoreContext storeContext
            , IStaticCacheManager staticCacheManager
            , ISettingService settingService
            , IWebHelper webHelper)
        {
            _storeContext = storeContext;
            _staticCacheManager = staticCacheManager;
            _settingService = settingService;
            _webHelper = webHelper;
        }

        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            var store = await _storeContext.GetCurrentStoreAsync();
            var helloWorldSettings = await _settingService.LoadSettingAsync<HelloWorldSettings>(store.Id);
            var model = new PublicInfoModel
            {
                DisplayText = helloWorldSettings.DisplayText
            };

            if (string.IsNullOrEmpty(model.DisplayText))
                return Content("");

            return View("~/Plugins/Widgets.HelloWorld/Views/PublicInfo.cshtml", model);
        }
    }
}
