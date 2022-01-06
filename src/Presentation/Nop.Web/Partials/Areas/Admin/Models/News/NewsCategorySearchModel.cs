using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.News
{
    /// <summary>
    /// Represents a news item search model
    /// </summary>
    public partial record NewsCategorySearchModel : BaseSearchModel
    {
        #region Ctor

        public NewsCategorySearchModel()
        {
            AvailableStores = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.ContentManagement.News.NewsCategories.List.SearchStore")]
        public int SearchStoreId { get; set; }

        public IList<SelectListItem> AvailableStores { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.News.NewsCategories.List.SearchName")]
        public string SearchName { get; set; }

        public bool HideStoresList { get; set; }

        #endregion
    }
}