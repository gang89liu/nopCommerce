using System;
using System.Collections.Generic;
using Nop.Web.Framework.Models;

namespace Nop.Web.Models.News
{
    public partial record NewsCategoryModel : BaseNopEntityModel
    {
        public NewsCategoryModel()
        {
        }

        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public string SeName { get; set; }
        public string Name { get; set; }
        public string ParentCategoryName { get; set; }
        public int ParentCategoryId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}