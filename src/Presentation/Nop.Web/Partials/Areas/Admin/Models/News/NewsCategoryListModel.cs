using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.News
{
    /// <summary>
    /// Represents a news category list model
    /// </summary>
    public partial record NewsCategoryListModel : BasePagedListModel<NewsCategoryModel>
    {
    }
}