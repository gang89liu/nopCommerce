using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;
using Nop.Web.Areas.Admin.Models.Catalog;

namespace Nop.Web.Areas.Admin.Models.News
{
    /// <summary>
    /// Represents a news category model
    /// </summary>
    public partial record NewsCategoryModel : BaseNopEntityModel, ILocalizedModel<NewsCategoryLocalizedModel>, IStoreMappingSupportedModel
    {
        #region Ctor

        public NewsCategoryModel()
        {
            Locales = new List<NewsCategoryLocalizedModel>();
            SelectedStoreIds = new List<int>();
            AvailableStores = new List<SelectListItem>();
            AvailableCategories = new List<SelectListItem>();
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

        public IList<SelectListItem> AvailableCategories { get; set; }
        [NopResourceDisplayName("Admin.ContentManagement.News.NewsCategories.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

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

        public IList<NewsCategoryLocalizedModel> Locales { get; set; }

        #endregion
    }

    public partial record NewsCategoryLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.News.NewsCategories.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.News.NewsCategories.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.News.NewsCategories.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.News.NewsCategories.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.News.NewsCategories.Fields.SeName")]
        public string SeName { get; set; }
    }
}