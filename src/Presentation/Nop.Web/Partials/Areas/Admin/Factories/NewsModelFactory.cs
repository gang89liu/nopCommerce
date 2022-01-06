using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.News;
using Nop.Core.Html;
using Nop.Services.Customers;
using Nop.Services.Helpers;
using Nop.Services.Localization;
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
            var newsCategories = await _newsService.GetAllNewsCategoryAsync(showHidden: true,
                storeId: searchModel.SearchStoreId,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize,
                name: searchModel.SearchName);

            //prepare list model
            var model = await new NewsCategoryListModel().PrepareToGridAsync(searchModel, newsCategories, () =>
            {
                return newsCategories.SelectAwait(async newsCategory =>
                {
                    //fill in model values from the entity
                    var newsCategoryModel = newsCategory.ToModel<NewsCategoryModel>();

                    //convert dates to the user time
                    newsCategoryModel.CreatedOn = await _dateTimeHelper.ConvertToUserTimeAsync(newsCategory.CreatedOnUtc, DateTimeKind.Utc);

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
            //fill in model values from the entity
            if (newsCategory != null)
            {
                if (model == null)
                {
                    model = newsCategory.ToModel<NewsCategoryModel>();
                    model.SeName = await _urlRecordService.GetSeNameAsync(newsCategory, null, true, false);
                }
            }

            //set default values for the new model
            if (newsCategory == null)
            {
                model.Published = true;
            }

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