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
        Task<IPagedList<NewsCategory>> GetAllNewsCategoryAsync(int languageId = 0, int storeId = 0,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false, string name = null);

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
        /// Gets all categories filtered by parent category identifier
        /// </summary>
        /// <param name="parentCategoryId">Parent category identifier</param>
        /// <param name="languageId">
        /// <param name="storeId">
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the categories
        /// </returns>
        Task<IList<NewsCategory>> GetAllCategoriesByParentCategoryIdAsync(int parentCategoryId,
            int languageId = 0, int storeId = 0,
            bool showHidden = false);

        #endregion
    }
}