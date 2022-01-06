using Nop.Core.Domain.News;
using Nop.Services.Caching;
using System.Threading.Tasks;

namespace Nop.Services.News.Caching
{
    /// <summary>
    /// Represents a news category cache event consumer
    /// </summary>
    public partial class NewsCategoryCacheEventConsumer : CacheEventConsumer<NewsCategory>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="entityEventType">Entity event type</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(NewsCategory entity, EntityEventType entityEventType)
        {
            await base.ClearCacheAsync(entity, entityEventType);
        }
    }
}