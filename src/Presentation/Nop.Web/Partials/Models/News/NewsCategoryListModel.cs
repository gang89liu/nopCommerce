using System.Collections.Generic;
using Nop.Web.Framework.Models;

namespace Nop.Web.Models.News
{
    public partial record NewsCategoryListModel : BaseNopModel
    {
        public NewsCategoryListModel()
        {
            PagingFilteringContext = new NewsCategoryPagingFilteringModel();
            NewsCategories = new List<NewsCategoryModel>();
        }

        public NewsCategoryPagingFilteringModel PagingFilteringContext { get; set; }
        public IList<NewsCategoryModel> NewsCategories { get; set; }
    }
}