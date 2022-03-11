using Nop.Core.Caching;
using Nop.Core.Domain.News;

namespace Nop.Services.News
{
    /// <summary>
    /// Represents default values related to orders services
    /// </summary>
    public static partial class NopNewsDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Key for number of news comments
        /// </summary>
        /// <remarks>
        /// {0} : news item ID
        /// {1} : store ID
        /// {2} : are only approved comments?
        /// </remarks>
        public static CacheKey NewsCommentsNumberCacheKey => new("Nop.newsitem.comments.number.{0}-{1}-{2}", NewsCommentsNumberPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        /// <remarks>
        /// {0} : news item ID
        /// </remarks>
        public static string NewsCommentsNumberPrefix => "Nop.newsitem.comments.number.{0}";



        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : parent category ID
        /// {1} : show hidden records?
        /// {3} : store ID
        /// </remarks>
        public static CacheKey NewsCategoriesByParentCategoryCacheKey => new CacheKey("Nop.newscategory.byparent.{0}-{1}-{2}", NewsCategoriesByParentCategoryPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        /// <remarks>
        /// {0} : parent category ID
        /// </remarks>
        public static string NewsCategoriesByParentCategoryPrefix => "Nop.newscategory.byparent.{0}";



        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : parent category id
        /// {1} : roles of the current user
        /// {2} : current store ID
        /// {3} : show hidden records?
        /// </remarks>
        public static CacheKey NewsCategoriesChildIdsCacheKey => new CacheKey("Nop.newscategory.childids.{0}-{1}-{2}-{3}", NewsCategoriesChildIdsPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        /// <remarks>
        /// {0} : parent category ID
        /// </remarks>
        public static string NewsCategoriesChildIdsPrefix => "Nop.newscategory.childids.{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : current store ID
        /// {1} : show hidden records?
        /// </remarks>
        public static CacheKey NewsCategoriesAllCacheKey => new CacheKey("Nop.newscategory.all.{0}-{1}", NopEntityCacheDefaults<NewsCategory>.AllPrefix);


        /// <summary>
        /// Key for caching of category breadcrumb
        /// </summary>
        /// <remarks>
        /// {0} : category id
        /// {2} : current store ID
        /// {3} : language ID
        /// </remarks>
        public static CacheKey NewsCategoryBreadcrumbCacheKey => new CacheKey("Nop.newscategory.breadcrumb.{0}-{1}-{2}", NewsCategoryBreadcrumbPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string NewsCategoryBreadcrumbPrefix => "Nop.newscategory.breadcrumb.";
        #endregion
    }
}