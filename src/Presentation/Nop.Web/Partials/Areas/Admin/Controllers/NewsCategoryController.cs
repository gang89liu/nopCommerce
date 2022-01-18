using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.News;
using Nop.Core.Events;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.News;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Stores;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.News;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Web.Areas.Admin.Controllers
{
    public partial class NewsCategoryController : BaseAdminController
    {
        #region Fields

        private readonly ICustomerActivityService _customerActivityService;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILocalizationService _localizationService;
        private readonly INewsModelFactory _newsModelFactory;
        private readonly INewsService _newsService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IStoreService _storeService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly ILocalizedEntityService _localizedEntityService;

        #endregion

        #region Ctor

        public NewsCategoryController(ICustomerActivityService customerActivityService,
            IEventPublisher eventPublisher,
            ILocalizationService localizationService,
            INewsModelFactory newsModelFactory,
            INewsService newsService,
            INotificationService notificationService,
            IPermissionService permissionService,
            IStoreMappingService storeMappingService,
            IStoreService storeService,
            IUrlRecordService urlRecordService,
            ILocalizedEntityService localizedEntityService)
        {
            _customerActivityService = customerActivityService;
            _eventPublisher = eventPublisher;
            _localizationService = localizationService;
            _newsModelFactory = newsModelFactory;
            _newsService = newsService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _storeMappingService = storeMappingService;
            _storeService = storeService;
            _urlRecordService = urlRecordService;
            _localizedEntityService = localizedEntityService;
        }

        #endregion

        #region Utilities

        /// <returns>A task that represents the asynchronous operation</returns>
        protected virtual async Task SaveStoreMappingsAsync(NewsCategory newsCategory, NewsCategoryModel model)
        {
            newsCategory.LimitedToStores = model.SelectedStoreIds.Any();
            await _newsService.UpdateNewsCategoryAsync(newsCategory);

            var existingStoreMappings = await _storeMappingService.GetStoreMappingsAsync(newsCategory);
            var allStores = await _storeService.GetAllStoresAsync();
            foreach (var store in allStores)
            {
                if (model.SelectedStoreIds.Contains(store.Id))
                {
                    //new store
                    if (existingStoreMappings.Count(sm => sm.StoreId == store.Id) == 0)
                        await _storeMappingService.InsertStoreMappingAsync(newsCategory, store.Id);
                }
                else
                {
                    //remove store
                    var storeMappingToDelete = existingStoreMappings.FirstOrDefault(sm => sm.StoreId == store.Id);
                    if (storeMappingToDelete != null)
                        await _storeMappingService.DeleteStoreMappingAsync(storeMappingToDelete);
                }
            }
        }

        /// <returns>A task that represents the asynchronous operation</returns>
        protected virtual async Task UpdateLocalesAsync(NewsCategory category, NewsCategoryModel model)
        {
            foreach (var localized in model.Locales)
            {
                await _localizedEntityService.SaveLocalizedValueAsync(category,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);

                await _localizedEntityService.SaveLocalizedValueAsync(category,
                    x => x.MetaKeywords,
                    localized.MetaKeywords,
                    localized.LanguageId);

                await _localizedEntityService.SaveLocalizedValueAsync(category,
                    x => x.MetaDescription,
                    localized.MetaDescription,
                    localized.LanguageId);

                await _localizedEntityService.SaveLocalizedValueAsync(category,
                    x => x.MetaTitle,
                    localized.MetaTitle,
                    localized.LanguageId);

                //search engine name
                var seName = await _urlRecordService.ValidateSeNameAsync(category, localized.SeName, localized.Name, false);
                await _urlRecordService.SaveSlugAsync(category, seName, localized.LanguageId);
            }
        }

        #endregion

        #region Methods        

        #region News categories
        public virtual IActionResult Index()
        {
            return RedirectToAction("NewsCategories");
        }

        public virtual async Task<IActionResult> NewsCategories()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            //prepare model
            var model = await _newsModelFactory.PrepareNewsCategorySearchModelAsync(new NewsCategorySearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> List(NewsCategorySearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageNews))
                return await AccessDeniedDataTablesJson();

            //prepare model
            var model = await _newsModelFactory.PrepareNewsCategoryListModelAsync(searchModel);

            return Json(model);
        }

        public virtual async Task<IActionResult> NewsCategoryCreate()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            //prepare model
            var model = await _newsModelFactory.PrepareNewsCategoryModelAsync(new NewsCategoryModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual async Task<IActionResult> NewsCategoryCreate(NewsCategoryModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var newsCategory = model.ToEntity<NewsCategory>();
                newsCategory.CreatedOnUtc = DateTime.UtcNow;
                await _newsService.InsertNewsCategoryAsync(newsCategory);

                //activity log
                await _customerActivityService.InsertActivityAsync("AddNewNewsCategory",
                    string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewNewsCategory"), newsCategory.Id), newsCategory);

                //search engine name
                var seName = await _urlRecordService.ValidateSeNameAsync(newsCategory, model.SeName, model.Name, true);
                await _urlRecordService.SaveSlugAsync(newsCategory, seName, 0);

                //locales
                await UpdateLocalesAsync(newsCategory, model);

                //Stores
                await SaveStoreMappingsAsync(newsCategory, model);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.ContentManagement.News.NewsCategories.Added"));

                if (!continueEditing)
                    return RedirectToAction("NewsCategories");

                return RedirectToAction("NewsCategoryEdit", new { id = newsCategory.Id });
            }

            //prepare model
            model = await _newsModelFactory.PrepareNewsCategoryModelAsync(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual async Task<IActionResult> NewsCategoryEdit(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            //try to get a news item with the specified id
            var newsCategory = await _newsService.GetNewsCategoryByIdAsync(id);
            if (newsCategory == null)
                return RedirectToAction("NewsCategories");

            //prepare model
            var model = await _newsModelFactory.PrepareNewsCategoryModelAsync(null, newsCategory);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual async Task<IActionResult> NewsCategoryEdit(NewsCategoryModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            //try to get a news item with the specified id
            var newsCategory = await _newsService.GetNewsCategoryByIdAsync(model.Id);
            if (newsCategory == null)
                return RedirectToAction("NewsCategories");

            if (ModelState.IsValid)
            {
                newsCategory = model.ToEntity(newsCategory);
                await _newsService.UpdateNewsCategoryAsync(newsCategory);

                //activity log
                await _customerActivityService.InsertActivityAsync("EditNewsCatgeory",
                    string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditNewsCategory"), newsCategory.Id), newsCategory);

                //search engine name
                var seName = await _urlRecordService.ValidateSeNameAsync(newsCategory, model.SeName, model.Name, true);
                await _urlRecordService.SaveSlugAsync(newsCategory, seName, 0);

                //locales
                await UpdateLocalesAsync(newsCategory, model);

                //stores
                await SaveStoreMappingsAsync(newsCategory, model);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.ContentManagement.News.NewsCategories.Updated"));

                if (!continueEditing)
                    return RedirectToAction("NewsCategories");

                return RedirectToAction("NewsCategoryEdit", new { id = newsCategory.Id });
            }

            //prepare model
            model = await _newsModelFactory.PrepareNewsCategoryModelAsync(model, newsCategory, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> NewsCategoryDelete(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();

            //try to get a news item with the specified id
            var newsCategory = await _newsService.GetNewsCategoryByIdAsync(id);
            if (newsCategory == null)
                return RedirectToAction("NewsCategories");

            await _newsService.DeleteNewsCategoryAsync(newsCategory);

            //activity log
            await _customerActivityService.InsertActivityAsync("DeleteNewsCategory",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteNewsCategory"), newsCategory.Id), newsCategory);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.ContentManagement.News.NewsCategories.Deleted"));

            return RedirectToAction("NewsCategories");
        }

        #endregion

        #endregion
    }
}