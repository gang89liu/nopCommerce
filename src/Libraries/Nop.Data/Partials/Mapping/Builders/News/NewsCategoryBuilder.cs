using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.News;
using Nop.Data.Extensions;

namespace Nop.Data.Mapping.Builders.News
{
    /// <summary>
    /// Represents a news item entity builder
    /// </summary>
    public partial class NewsCategoryBuilder : NopEntityBuilder<NewsCategory>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(NewsCategory.Name)).AsString(400).NotNullable()
                .WithColumn(nameof(NewsCategory.MetaKeywords)).AsString(400).Nullable()
                .WithColumn(nameof(NewsCategory.MetaTitle)).AsString(400).Nullable()
                ;
        }

        #endregion
    }
}