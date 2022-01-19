using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.News;
using Nop.Core.Domain.Security;
using Nop.Core.Html;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.News;
using Nop.Services.Seo;
using Nop.Services.Stores;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.News;
using Nop.Web.Framework.Extensions;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the news model factory implementation
    /// </summary>
    public partial class NewsModelFactory : INewsModelFactory
    {
        #region Fields
        private readonly ILocalizedModelFactory _localizedModelFactory;

        #endregion

        #region Ctor
        public NewsModelFactory(CatalogSettings catalogSettings,
            IBaseAdminModelFactory baseAdminModelFactory,
            ICustomerService customerService,
            IDateTimeHelper dateTimeHelper,
            ILanguageService languageService,
            ILocalizationService localizationService,
            INewsService newsService,
            IStoreMappingSupportedModelFactory storeMappingSupportedModelFactory,
            IStoreService storeService,
            IUrlRecordService urlRecordService,
            ILocalizedModelFactory localizedModelFactory)
            :this(catalogSettings,
             baseAdminModelFactory,
             customerService,
             dateTimeHelper,
             languageService,
             localizationService,
             newsService,
             storeMappingSupportedModelFactory,
             storeService,
             urlRecordService)
        {
            _localizedModelFactory = localizedModelFactory;
        }
        #endregion
        #region Methods

        /// <summary>
        /// Prepare paged news category list model
        /// </summary>
        /// <param name="searchModel">News category search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the news category list model
        /// </returns>
        public virtual async Task<NewsCategoryListModel> PrepareNewsCategoryListModelAsync(NewsCategorySearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get news categories
            var newsCategories = await _newsService.GetAllNewsCategoryAsync(categoryName: searchModel.SearchName, showHidden: true,
                storeId: searchModel.SearchStoreId,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

            //prepare list model
            var model = await new NewsCategoryListModel().PrepareToGridAsync(searchModel, newsCategories, () =>
            {
                return newsCategories.SelectAwait(async newsCategory =>
                {
                    //fill in model values from the entity
                    var newsCategoryModel = newsCategory.ToModel<NewsCategoryModel>();

                    //convert dates to the user time
                    newsCategoryModel.CreatedOn = await _dateTimeHelper.ConvertToUserTimeAsync(newsCategory.CreatedOnUtc, DateTimeKind.Utc);

                    newsCategoryModel.Breadcrumb = await _newsService.GetFormattedBreadCrumbAsync(newsCategory);
                    //fill in additional values (not existing in the entity)
                    newsCategoryModel.SeName = await _urlRecordService.GetSeNameAsync(newsCategory, null, true, false);

                    return newsCategoryModel;
                });
            });

            return model;
        }

        /// <summary>
        /// Prepare news category model
        /// </summary>
        /// <param name="model">News category model</param>
        /// <param name="newsCategory">News category</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the news item model
        /// </returns>
        public virtual async Task<NewsCategoryModel> PrepareNewsCategoryModelAsync(NewsCategoryModel model, NewsCategory newsCategory, bool excludeProperties = false)
        {
            Action<NewsCategoryLocalizedModel, int> localizedModelConfiguration = null;
            //fill in model values from the entity
            if (newsCategory != null)
            {
                if (model == null)
                {
                    model = newsCategory.ToModel<NewsCategoryModel>();
                    model.SeName = await _urlRecordService.GetSeNameAsync(newsCategory, null, true, false);
                }

                //define localized model configuration action
                localizedModelConfiguration = async (locale, languageId) =>
                {
                    locale.Name = await _localizationService.GetLocalizedAsync(newsCategory, entity => entity.Name, languageId, false, false);
                    locale.MetaKeywords = await _localizationService.GetLocalizedAsync(newsCategory, entity => entity.MetaKeywords, languageId, false, false);
                    locale.MetaDescription = await _localizationService.GetLocalizedAsync(newsCategory, entity => entity.MetaDescription, languageId, false, false);
                    locale.MetaTitle = await _localizationService.GetLocalizedAsync(newsCategory, entity => entity.MetaTitle, languageId, false, false);
                    locale.SeName = await _urlRecordService.GetSeNameAsync(newsCategory, languageId, false, false);
                };
            }

            //set default values for the new model
            if (newsCategory == null)
            {
                model.Published = true;
            }

            //prepare localized models
            if (!excludeProperties)
                model.Locales = await _localizedModelFactory.PrepareLocalizedModelsAsync(localizedModelConfiguration);


            //prepare available parent categories
            await _baseAdminModelFactory.PrepareNewsCategoriesAsync(model.AvailableCategories,
                defaultItemText: await _localizationService.GetResourceAsync("Admin.Contentmanagement.News.NewsCategories.Fields.Parent.None"));

            //prepare available stores
            await _storeMappingSupportedModelFactory.PrepareModelStoresAsync(model, newsCategory, excludeProperties);

            return model;
        }

        /// <summary>
        /// Prepare news category search model
        /// </summary>
        /// <param name="searchModel">News category search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the news category search model
        /// </returns>
        public virtual async Task<NewsCategorySearchModel> PrepareNewsCategorySearchModelAsync(NewsCategorySearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare available stores
            await _baseAdminModelFactory.PrepareStoresAsync(searchModel.AvailableStores);

            searchModel.HideStoresList = _catalogSettings.IgnoreStoreLimitations || searchModel.AvailableStores.SelectionIsNotPossible();

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        #endregion
    }
}