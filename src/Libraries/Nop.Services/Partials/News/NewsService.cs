using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.News;
using Nop.Data;
using Nop.Services.Localization;
using Nop.Services.Stores;

namespace Nop.Services.News
{
    /// <summary>
    /// News service
    /// </summary>
    public partial class NewsService : INewsService
    {

        #region Fields

        private readonly IRepository<NewsCategory> _newsCategoryRepository;
        private readonly IStoreContext _storeContext;
        private readonly IWorkContext _workContext;
        private readonly ILocalizationService _localizationService;

        #endregion

        #region Ctor

        public NewsService(
            IRepository<NewsComment> newsCommentRepository,
            IRepository<NewsItem> newsItemRepository,
            IRepository<NewsCategory> newsCategoryRepository,
            IStaticCacheManager staticCacheManager,
            IStoreMappingService storeMappingService,
            IStoreContext storeContext,
            IWorkContext workContext,
            ILocalizationService localizationService)
            : this(newsCommentRepository, newsItemRepository, staticCacheManager
                  , storeMappingService)
        {
            _newsCategoryRepository = newsCategoryRepository;
            _storeContext = storeContext;
            _workContext = workContext;
            _localizationService = localizationService;
        }

        #endregion

        #region Methods

        #region NewsCategory

        /// <summary>
        /// Deletes a news category
        /// </summary>
        /// <param name="newsCategory">News item</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteNewsCategoryAsync(NewsCategory newsCategory)
        {
            await _newsCategoryRepository.DeleteAsync(newsCategory);

            //reset a "Parent category" property of all child subcategories
            var subcategories = await GetAllCategoriesByParentCategoryIdAsync(newsCategory.Id, showHidden: true);
            foreach(var subcategory in subcategories)
            {
                subcategory.ParentCategoryId = 0;
                await UpdateNewsCategoryAsync(subcategory);
            }

        }

        /// <summary>
        /// Delete Categories
        /// </summary>
        /// <param name="categories">Categories</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteCategoriesAsync(IList<NewsCategory> categories)
        {
            if (categories == null)
                throw new ArgumentNullException(nameof(categories));

            foreach (var category in categories)
                await DeleteNewsCategoryAsync(category);
        }



        /// <summary>
        /// Gets a news category
        /// </summary>
        /// <param name="newsCategoryId">The news identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the news
        /// </returns>
        public virtual async Task<NewsCategory> GetNewsCategoryByIdAsync(int newsCategoryId)
        {
            return await _newsCategoryRepository.GetByIdAsync(newsCategoryId, cache => default);
        }

        /// <summary>
        /// Gets all news category
        /// </summary>
        /// <param name="languageId">Language identifier; 0 if you want to get all records</param>
        /// <param name="storeId">Store identifier; 0 if you want to get all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="name">Filter by news item title</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the news items
        /// </returns>
        public virtual async Task<IPagedList<NewsCategory>> GetAllNewsCategoryAsync(string categoryName, int languageId = 0, int storeId = 0,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false, bool? overridePublished = null)
        {
            var news = await _newsCategoryRepository.GetAllPagedAsync(async query =>
            {
              
                if (!string.IsNullOrEmpty(categoryName))
                    query = query.Where(n => n.Name.Contains(categoryName));

                if (!showHidden)
                {
                    query = query.Where(n => n.Published);
                }
                else if (overridePublished.HasValue)
                    query = query.Where(c => c.Published == overridePublished.Value);

                //apply store mapping constraints
                query = await _storeMappingService.ApplyStoreMapping(query, storeId);

                query = query.Where(c => !c.Deleted);

                return query.OrderBy(c => c.ParentCategoryId).ThenBy(c => c.DisplayOrder).ThenBy(c => c.Id);
            }, pageIndex, pageSize);

            return news;
        }


        /// <summary>
        /// Gets all news category
        /// </summary>
        /// <param name="languageId">Language identifier; 0 if you want to get all records</param>
        /// <param name="storeId">Store identifier; 0 if you want to get all records</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the news items
        /// </returns>
        public virtual async Task<IList<NewsCategory>> GetAllNewsCategoriesAsync(int languageId = 0, int storeId = 0,
            bool showHidden = false)
        {
            var key = _staticCacheManager.PrepareKeyForDefaultCache(NopNewsDefaults.NewsCategoriesAllCacheKey,
                storeId,
                showHidden);

            var categories = await _staticCacheManager
                .GetAsync(key, async () => (await GetAllNewsCategoryAsync(string.Empty, languageId, storeId, showHidden: showHidden)).ToList());

            return categories;
        }


        /// <summary>
        /// Gets all categories filtered by parent category identifier
        /// </summary>
        /// <param name="parentCategoryId">Parent category identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the categories
        /// </returns>
        public virtual async Task<IList<NewsCategory>> GetAllNewsCategoriesByParentCategoryIdAsync(int parentCategoryId,
            bool showHidden = false)
        {
            var store = await _storeContext.GetCurrentStoreAsync();
            var categories = await _newsCategoryRepository.GetAllAsync(async query =>
            {
                if (!showHidden)
                {
                    query = query.Where(c => c.Published);

                    //apply store mapping constraints
                    query = await _storeMappingService.ApplyStoreMapping(query, store.Id);
                }

                query = query.Where(c => !c.Deleted && c.ParentCategoryId == parentCategoryId);

                return query.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id);
            }, cache => cache.PrepareKeyForDefaultCache(NopNewsDefaults.NewsCategoriesByParentCategoryCacheKey,
                parentCategoryId, showHidden, store));

            return categories;
        }

        /// <summary>
        /// Gets child category identifiers
        /// </summary>
        /// <param name="parentCategoryId">Parent category identifier</param>
        /// <param name="storeId">Store identifier; 0 if you want to get all records</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the category identifiers
        /// </returns>
        public virtual async Task<IList<int>> GetChildNewsCategoryIdsAsync(int parentCategoryId, int storeId = 0, bool showHidden = false)
        {
            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(NopNewsDefaults.NewsCategoriesChildIdsCacheKey,
                parentCategoryId,
                storeId,
                showHidden);

            return await _staticCacheManager.GetAsync(cacheKey, async () =>
            {
                //little hack for performance optimization
                //there's no need to invoke "GetAllCategoriesByParentCategoryId" multiple times (extra SQL commands) to load childs
                //so we load all categories at once (we know they are cached) and process them server-side
                var categoriesIds = new List<int>();
                var categories = (await GetAllNewsCategoriesAsync(storeId: storeId, showHidden: showHidden))
                    .Where(c => c.ParentCategoryId == parentCategoryId)
                    .Select(c => c.Id)
                    .ToList();
                categoriesIds.AddRange(categories);
                categoriesIds.AddRange(await categories.SelectManyAwait(async cId => await GetChildNewsCategoryIdsAsync(cId, storeId, showHidden)).ToListAsync());

                return categoriesIds;
            });
        }

        /// <summary>
        /// Inserts a news category
        /// </summary>
        /// <param name="newsCategory">News category</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertNewsCategoryAsync(NewsCategory newsCategory)
        {
            await _newsCategoryRepository.InsertAsync(newsCategory);
        }

        /// <summary>
        /// Updates the news category
        /// </summary>
        /// <param name="newsCategory">News category</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateNewsCategoryAsync(NewsCategory newsCategory)
        {
            await _newsCategoryRepository.UpdateAsync(newsCategory);
        }

        public virtual async Task<IList<NewsCategory>> GetAllCategoriesByParentCategoryIdAsync(int parentCategoryId, 
            int languageId = 0, int storeId = 0,
            bool showHidden = false)
        {
            var categories = await _newsCategoryRepository.GetAllAsync(async query =>
            {
           
                if (!showHidden)
                {
                    query = query.Where(c => c.Published);

                    //apply store mapping constraints
                    query = await _storeMappingService.ApplyStoreMapping(query, storeId);
                }

                query = query.Where(c => !c.Deleted && c.ParentCategoryId == parentCategoryId);
                return query.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id);
            }, cache => cache.PrepareKeyForDefaultCache(NopNewsDefaults.NewsCategoriesByParentCategoryCacheKey
                        , parentCategoryId, showHidden, languageId, storeId));

            return categories;
      
        }


        /// <summary>
        /// Get formatted category breadcrumb 
        /// Note: ACL and store mapping is ignored
        /// </summary>
        /// <param name="category">Category</param>
        /// <param name="allCategories">All categories</param>
        /// <param name="separator">Separator</param>
        /// <param name="languageId">Language identifier for localization</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the formatted breadcrumb
        /// </returns>
        public virtual async Task<string> GetFormattedBreadCrumbAsync(NewsCategory category, IList<NewsCategory> allCategories = null,
            string separator = ">>", int languageId = 0)
        {
            var result = string.Empty;

            var breadcrumb = await GetCategoryBreadCrumbAsync(category, allCategories, true);
            for (var i = 0; i <= breadcrumb.Count - 1; i++)
            {
                var categoryName = await _localizationService.GetLocalizedAsync(breadcrumb[i], x => x.Name, languageId);
                result = string.IsNullOrEmpty(result) ? categoryName : $"{result} {separator} {categoryName}";
            }

            return result;
        }

        /// <summary>
        /// Get category breadcrumb 
        /// </summary>
        /// <param name="category">Category</param>
        /// <param name="allCategories">All categories</param>
        /// <param name="showHidden">A value indicating whether to load hidden records</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the category breadcrumb 
        /// </returns>
        public virtual async Task<IList<NewsCategory>> GetCategoryBreadCrumbAsync(NewsCategory category, IList<NewsCategory> allCategories = null, bool showHidden = false)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            var breadcrumbCacheKey = _staticCacheManager.PrepareKeyForDefaultCache(NopNewsDefaults.NewsCategoryBreadcrumbCacheKey,
                category,
                await _storeContext.GetCurrentStoreAsync(),
                await _workContext.GetWorkingLanguageAsync());

            return await _staticCacheManager.GetAsync(breadcrumbCacheKey, async () =>
            {
                var result = new List<NewsCategory>();

                //used to prevent circular references
                var alreadyProcessedCategoryIds = new List<int>();

                while (category != null && //not null
                       !category.Deleted && //not deleted
                       (showHidden || category.Published) && //published
                       (showHidden || await _storeMappingService.AuthorizeAsync(category)) && //Store mapping
                       !alreadyProcessedCategoryIds.Contains(category.Id)) //prevent circular references
                {
                    result.Add(category);

                    alreadyProcessedCategoryIds.Add(category.Id);

                    category = allCategories != null
                        ? allCategories.FirstOrDefault(c => c.Id == category.ParentCategoryId)
                        : await GetNewsCategoryByIdAsync(category.ParentCategoryId);
                }

                result.Reverse();

                return result;
            });
        }

        #endregion

        #endregion
    }
}