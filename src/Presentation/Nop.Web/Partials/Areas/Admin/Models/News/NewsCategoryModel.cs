using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.News
{
    /// <summary>
    /// Represents a news category model
    /// </summary>
    public partial record NewsCategoryModel : BaseNopEntityModel, IStoreMappingSupportedModel
    {
        #region Ctor

        public NewsCategoryModel()
        {
            SelectedStoreIds = new List<int>();
            AvailableStores = new List<SelectListItem>();
        }

        #endregion

        #region Properties
        //store mapping
        [NopResourceDisplayName("Admin.ContentManagement.News.NewsCategories.Fields.LimitedToStores")]
        public IList<int> SelectedStoreIds { get; set; }

        public IList<SelectListItem> AvailableStores { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.News.NewsCategories.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.News.NewsCategories.Fields.ParentCategoryId")]
        public int ParentCategoryId { get; set; }


        [NopResourceDisplayName("Admin.ContentManagement.News.NewsCategories.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.News.NewsCategories.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.News.NewsCategories.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.News.NewsCategories.Fields.SeName")]
        public string SeName { get; set; }


        [NopResourceDisplayName("Admin.ContentManagement.News.NewsCategories.Fields.Published")]
        public bool Published { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.News.NewsCategories.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        #endregion
    }
}