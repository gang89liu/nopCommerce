using System.Collections.Generic;
using Nop.Web.Framework.Models;

namespace Nop.Web.Models.News
{
    public partial record HomepageNewsCategoriesModel : BaseNopModel
    {
        public HomepageNewsCategoriesModel()
        {
            NewsCategories = new List<NewsCategoryModel>();
        }

        public int WorkingLanguageId { get; set; }
        public IList<NewsCategoryModel> NewsCategories { get; set; }
    }
}