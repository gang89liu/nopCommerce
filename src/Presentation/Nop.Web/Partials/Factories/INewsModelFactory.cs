using System.Threading.Tasks;
using Nop.Core.Domain.News;
using Nop.Web.Models.News;

namespace Nop.Web.Factories
{
    /// <summary>
    /// Represents the interface of the news model factory
    /// </summary>
    public partial interface INewsModelFactory
    {
        /// <summary>
        /// Prepare the news category model
        /// </summary>
        /// <param name="model">News category model</param>
        /// <param name="newsCategory">News category</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the news category model
        /// </returns>
        Task<NewsCategoryModel> PrepareNewsCategoryModelAsync(NewsCategoryModel model, NewsCategory newsCategory);
        /// <summary>
        /// Prepare the home page news categories model
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the home page news categories model
        /// </returns>
         Task<HomepageNewsCategoriesModel> PrepareHomepageNewsCategoriesModelAsync();

        /// <summary>
        /// Prepare the news item category model
        /// </summary>
        /// <param name="command">News paging filtering model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the news item list model
        /// </returns>
        Task<NewsCategoryListModel> PrepareNewsCategoryListModelAsync(NewsPagingFilteringModel command);
    }
}
