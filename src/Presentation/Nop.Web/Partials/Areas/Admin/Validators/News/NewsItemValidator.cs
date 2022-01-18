using FluentValidation;
using Nop.Core.Domain.News;
using Nop.Data;
using Nop.Services.Localization;
using Nop.Services.Seo;
using Nop.Web.Areas.Admin.Models.News;
using Nop.Web.Framework.Validators;

namespace Nop.Web.Areas.Admin.Validators.News
{
    public partial class NewsCategoryValidator : BaseNopValidator<NewsCategoryModel>
    {
        public NewsCategoryValidator(ILocalizationService localizationService, INopDataProvider dataProvider)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Admin.ContentManagement.News.NewsItems.Fields.Title.Required"));

            RuleFor(x => x.SeName).Length(0, NopSeoDefaults.SearchEngineNameLength)
                .WithMessageAwait(localizationService.GetResourceAsync("Admin.SEO.SeName.MaxLengthValidation"), NopSeoDefaults.SearchEngineNameLength);

            SetDatabaseValidationRules<NewsCategory>(dataProvider);
        }
    }
}