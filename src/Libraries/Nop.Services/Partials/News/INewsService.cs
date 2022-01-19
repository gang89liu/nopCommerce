using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.News;

namespace Nop.Services.News
{
    /// <summary>
    /// News service interface
    /// </summary>
    public partial interface INewsService
    {
        #region NewsCategory

        /// <summary>
        /// Deletes a news category
        /// </summary>
        /// <param name="newsCategory">News item</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteNewsCategoryAsync(NewsCategory newsCategory);

        /// <summary>
        /// Gets a news category
        /// </summary>
        /// <param name="newsCategoryId">The news identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the news
        /// </returns>
        Task<NewsCategory> GetNewsCategoryByIdAsync(int newsCategoryId);

        /// <summary>
        /// Gets all news
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
        Task<IPagedList<NewsCategory>> GetAllNewsCategoryAsync(string categoryName, int languageId = 0, int storeId = 0,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false, bool? overridePublished = null);

        Task<IList<NewsCategory>> GetAllNewsCategoriesAsync(int languageId = 0, int storeId = 0, bool showHidden = false);

        Task<IList<NewsCategory>> GetAllNewsCategoriesByParentCategoryIdAsync(int parentCategoryId,
            bool showHidden = false);


        Task<IList<int>> GetChildNewsCategoryIdsAsync(int parentCategoryId, int storeId = 0, bool showHidden = false);


        Task<IList<NewsCategory>> GetAllCategoriesByParentCategoryIdAsync(int parentCategoryId,
            int languageId = 0, int storeId = 0,
            bool showHidden = false);
        /// <summary>
        /// Inserts a news category
        /// </summary>
        /// <param name="newsCategory">News item</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertNewsCategoryAsync(NewsCategory newsCategory);

        /// <summary>
        /// Updates the news category
        /// </summary>
        /// <param name="newsCategory">News item</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateNewsCategoryAsync(NewsCategory newsCategory);


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
        Task<string> GetFormattedBreadCrumbAsync(NewsCategory category, IList<NewsCategory> allCategories = null,
            string separator = ">>", int languageId = 0);

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
        Task<IList<NewsCategory>> GetCategoryBreadCrumbAsync(NewsCategory category, IList<NewsCategory> allCategories = null, bool showHidden = false);

        #endregion
    }
}