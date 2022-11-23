using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Nop.Core;
using Nop.Core.Infrastructure;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Tutorial.DistOfCustByCountry
{
    /// <summary>
    /// PLugin
    /// </summary>
    public class DistOfCustByCountryPlugin : BasePlugin, IAdminMenuPlugin
    {
        private readonly IWebHelper _webHelper;

        public DistOfCustByCountryPlugin(IWebHelper webHelper)
        {
            _webHelper = webHelper;
        }

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/DistOfCustByCountry/Configure";
        }

        public override async Task InstallAsync()
        {
            //Code you want to run while installing the plugin goes here.
            await base.InstallAsync();
        }

        public Task ManageSiteMapAsync(SiteMapNode rootNode)
        {
            var menuItem = new SiteMapNode()
            {
                Url = "~/Admin/DistOfCustByCountry/Configure",
                SystemName = "DistOfCustByCountry",
                IconClass = "far fa-dot-circle",
                Title = "用户统计",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", null } },
            };
            //var node = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Third party plugins");
            var node = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Reports");
            if(node != null)
            {
                node = node.ChildNodes.FirstOrDefault(x => x.SystemName == "Customers");
            }
            if (node != null)
                node.ChildNodes.Add(menuItem);
            else
                rootNode.ChildNodes.Add(menuItem);

            return Task.CompletedTask;
        }
        public override async Task UninstallAsync()
        {
            //Code you want to run while Uninstalling the plugin goes here.
            await base.UninstallAsync();
        }
    }
}