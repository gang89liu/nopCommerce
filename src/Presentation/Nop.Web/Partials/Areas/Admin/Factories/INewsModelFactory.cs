using System.Threading.Tasks;
using Nop.Core.Domain.News;
using Nop.Web.Areas.Admin.Models.News;

namespace Nop.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the news model factory
    /// </summary>
    public partial interface INewsModelFactory
    {
        Task<NewsCategoryListModel> PrepareNewsCategoryListModelAsync(NewsCategorySearchModel searchModel);
        Task<NewsCategoryModel> PrepareNewsCategoryModelAsync(NewsCategoryModel model, NewsCategory newsCategory, bool excludeProperties = false);

        Task<NewsCategorySearchModel> PrepareNewsCategorySearchModelAsync(NewsCategorySearchModel searchModel);
    }
}