using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.Widgets.Swiper.Infrastructure.Cache;
using Nop.Plugin.Widgets.Swiper.Models;
using Nop.Services.Configuration;
using Nop.Services.Media;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.Swiper.Components
{
    [ViewComponent(Name = "WidgetsSwiper")]
    public class WidgetsSwiperViewComponent : NopViewComponent
    {
        private readonly IStoreContext _storeContext;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly ISettingService _settingService;
        private readonly IPictureService _pictureService;
        private readonly IWebHelper _webHelper;

        public WidgetsSwiperViewComponent(IStoreContext storeContext, 
            IStaticCacheManager staticCacheManager, 
            ISettingService settingService, 
            IPictureService pictureService,
            IWebHelper webHelper)
        {
            _storeContext = storeContext;
            _staticCacheManager = staticCacheManager;
            _settingService = settingService;
            _pictureService = pictureService;
            _webHelper = webHelper;
        }

        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            var store = await _storeContext.GetCurrentStoreAsync();
            var swiperSettings = await _settingService.LoadSettingAsync<SwiperSettings>(store.Id);

            var model = new PublicInfoModel
            {
                Picture1Url = await GetPictureUrlAsync(swiperSettings.Picture1Id),
                Text1 = swiperSettings.Text1,
                Link1 = swiperSettings.Link1,
                AltText1 = swiperSettings.AltText1,

                Picture2Url = await GetPictureUrlAsync(swiperSettings.Picture2Id),
                Text2 = swiperSettings.Text2,
                Link2 = swiperSettings.Link2,
                AltText2 = swiperSettings.AltText2,

                Picture3Url = await GetPictureUrlAsync(swiperSettings.Picture3Id),
                Text3 = swiperSettings.Text3,
                Link3 = swiperSettings.Link3,
                AltText3 = swiperSettings.AltText3,

                Picture4Url = await GetPictureUrlAsync(swiperSettings.Picture4Id),
                Text4 = swiperSettings.Text4,
                Link4 = swiperSettings.Link4,
                AltText4 = swiperSettings.AltText4,

                Picture5Url = await GetPictureUrlAsync(swiperSettings.Picture5Id),
                Text5 = swiperSettings.Text5,
                Link5 = swiperSettings.Link5,
                AltText5 = swiperSettings.AltText5
            };

            if (string.IsNullOrEmpty(model.Picture1Url) && string.IsNullOrEmpty(model.Picture2Url) &&
                string.IsNullOrEmpty(model.Picture3Url) && string.IsNullOrEmpty(model.Picture4Url) &&
                string.IsNullOrEmpty(model.Picture5Url))
                //no pictures uploaded
                return Content("");

            return View("~/Plugins/Widgets.Swiper/Views/PublicInfo.cshtml", model);
        }

        /// <returns>A task that represents the asynchronous operation</returns>
        protected async Task<string> GetPictureUrlAsync(int pictureId)
        {
            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(ModelCacheEventConsumer.PICTURE_URL_MODEL_KEY, 
                pictureId, _webHelper.IsCurrentConnectionSecured() ? Uri.UriSchemeHttps : Uri.UriSchemeHttp);

            return await _staticCacheManager.GetAsync(cacheKey, async () =>
            {
                //little hack here. nulls aren't cacheable so set it to ""
                var url = await _pictureService.GetPictureUrlAsync(pictureId, showDefaultPicture: false) ?? "";
                return url;
            });
        }
    }
}
