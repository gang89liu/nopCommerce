using Nop.Core.Caching;

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
        public static CacheKey NewsCommentsNumberCacheKey => new CacheKey("Nop.newsitem.comments.number.{0}-{1}-{2}", NewsCommentsNumberPrefix);

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
        /// {2} : language ID
        /// {3} : store ID
        /// </remarks>
        public static CacheKey CategoriesByParentCategoryCacheKey => new CacheKey("Nop.news.category.byparent.{0}-{1}-{2}-{3}", CategoriesByParentCategoryPrefix);

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        /// <remarks>
        /// {0} : parent category ID
        /// </remarks>
        public static string CategoriesByParentCategoryPrefix => "Nop.news.category.byparent.{0}";
        #endregion
    }
}