using FluentMigrator;
using Nop.Data.Mapping;
using Nop.Data.Extensions;
using Nop.Core.Domain.News;

namespace Nop.Data.Migrations.UpgradeTo440
{
    [NopMigration("2021/03/08 21:26:08:9037680", "NewsItem. Add some new property", UpdateMigrationType.Data)]
    [SkipMigrationOnUpdate]
    public class AddNewsCategoryId : AutoReversingMigration
    {
        /// <summary>Collect the UP migration expressions</summary>
        public override void Up()
        {
            Create.Column(nameof(NewsItem.NewsCategoryId))
            .OnTable(nameof(NewsItem))
            .AsInt32()
            .Nullable();
        }
    }
}
