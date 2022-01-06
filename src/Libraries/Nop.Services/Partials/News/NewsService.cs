using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.News;
using Nop.Data;
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

        #endregion

        #region Ctor

        public NewsService(
            IRepository<NewsComment> newsCommentRepository,
            IRepository<NewsItem> newsItemRepository,
            IRepository<NewsCategory> newsCategoryRepository,
            IStaticCacheManager staticCacheManager,
            IStoreMappingService storeMappingService)
            : this(newsCommentRepository, newsItemRepository, staticCacheManager
                  , storeMappingService)
        {
            _newsCategoryRepository = newsCategoryRepository;
        }

        #endregion

        #region Methods

        #region News

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
        public virtual async Task<IPagedList<NewsCategory>> GetAllNewsCategoryAsync(int languageId = 0, int storeId = 0,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false, string name = null)
        {
            var news = await _newsCategoryRepository.GetAllPagedAsync(async query =>
            {
              
                if (!string.IsNullOrEmpty(name))
                    query = query.Where(n => n.Name.Contains(name));

                if (!showHidden)
                {
                    var utcNow = DateTime.UtcNow;
                    query = query.Where(n => n.Published);
                }

                //apply store mapping constraints
                query = await _storeMappingService.ApplyStoreMapping(query, storeId);

                return query.OrderByDescending(n =>  n.CreatedOnUtc);
            }, pageIndex, pageSize);

            return news;
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
            }, cache => cache.PrepareKeyForDefaultCache(NopNewsDefaults.CategoriesByParentCategoryCacheKey
                        , parentCategoryId, showHidden, languageId, storeId));

            return categories;
      
        }
        #endregion

        #endregion
    }
}