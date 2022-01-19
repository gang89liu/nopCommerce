using System;
using System.Linq;
using System.Threading.Tasks;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.News;
using Nop.Web.Infrastructure.Cache;
using Nop.Web.Models.News;

namespace Nop.Web.Factories
{
    /// <summary>
    /// Represents the news model factory
    /// </summary>
    public partial class NewsModelFactory : INewsModelFactory
    {
        #region Methods

        /// <summary>
        /// Prepare the news category model
        /// </summary>
        /// <param name="model">News category model</param>
        /// <param name="newsCategory">News category</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the news category model
        /// </returns>
        public virtual async Task<NewsCategoryModel> PrepareNewsCategoryModelAsync(NewsCategoryModel model, NewsCategory newsCategory)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (newsCategory == null)
                throw new ArgumentNullException(nameof(newsCategory));

            model.Id = newsCategory.Id;
            model.MetaTitle = newsCategory.MetaTitle;
            model.MetaDescription = newsCategory.MetaDescription;
            model.MetaKeywords = newsCategory.MetaKeywords;
            model.SeName = await _urlRecordService.GetSeNameAsync(newsCategory, null, ensureTwoPublishedLanguages: false);
            model.Name = newsCategory.Name;
            model.ParentCategoryId = newsCategory.ParentCategoryId;
            model.CreatedOn = await _dateTimeHelper.ConvertToUserTimeAsync(newsCategory.CreatedOnUtc, DateTimeKind.Utc);

            return model;
        }

        /// <summary>
        /// Prepare the home page news categories model
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the home page news categories model
        /// </returns>
        public virtual async Task<HomepageNewsCategoriesModel> PrepareHomepageNewsCategoriesModelAsync()
        {
            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(NopModelCacheDefaults.HomepageNewsModelKey, await _workContext.GetWorkingLanguageAsync(), await _storeContext.GetCurrentStoreAsync());
            var cachedModel = await _staticCacheManager.GetAsync(cacheKey, async () =>
            {
                var language = await _workContext.GetWorkingLanguageAsync();
                var store = await _storeContext.GetCurrentStoreAsync();
                var newsCategories = await _newsService.GetAllNewsCategoryAsync(string.Empty, language.Id, store.Id, 0, _newsSettings.MainPageNewsCount);

                return new HomepageNewsCategoriesModel
                {
                    WorkingLanguageId = language.Id,
                    NewsCategories = await newsCategories.SelectAwait(async newsCategory =>
                    {
                        var newsCategoryModel = new NewsCategoryModel();
                        await PrepareNewsCategoryModelAsync(newsCategoryModel, newsCategory);
                        return newsCategoryModel;
                    }).ToListAsync()
                };
            });
         return cachedModel;
        }

        /// <summary>
        /// Prepare the news item category model
        /// </summary>
        /// <param name="command">News paging filtering model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the news item list model
        /// </returns>
        public virtual async Task<NewsCategoryListModel> PrepareNewsCategoryListModelAsync(NewsPagingFilteringModel command)
        {
            if (command.PageSize <= 0)
                command.PageSize = _newsSettings.NewsArchivePageSize;
            if (command.PageNumber <= 0)
                command.PageNumber = 1;

            var language = await _workContext.GetWorkingLanguageAsync();
            var store = await _storeContext.GetCurrentStoreAsync();
            var newsCategories = await _newsService.GetAllNewsCategoryAsync(string.Empty, language.Id, store.Id, command.PageNumber - 1, command.PageSize);

            var model = new NewsCategoryListModel
            {
                NewsCategories = await newsCategories.SelectAwait(async newsCategory =>
                {
                    var newsCategoryModel = new NewsCategoryModel();
                    await PrepareNewsCategoryModelAsync(newsCategoryModel, newsCategory);
                    return newsCategoryModel;
                }).ToListAsync()
            };
            model.PagingFilteringContext.LoadPagedList(newsCategories);

            return model;
        }
        #endregion
    }
}