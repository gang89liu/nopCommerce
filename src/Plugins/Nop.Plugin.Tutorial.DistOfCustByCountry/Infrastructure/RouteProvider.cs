using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Tutorial.DistOfCustByCountry.Infrastructure
{
    /// <summary>
    /// Represents plugin route provider
    /// </summary>
    public class RouteProvider : IRouteProvider
    {
        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="endpointRouteBuilder">Route builder</param>
        public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            //endpointRouteBuilder.MapControllerRoute(name: "Plugins.Tutorial.DistOfCustByCountry.Configure",
            //"Plugins/Tutorial/DistOfCustByCountry/Configure",
            // defaults: new { controller = "DistOfCustByCountry", action = "Configure", area = AreaNames.Admin });

            //endpointRouteBuilder.MapControllerRoute("Plugin.Tutorial.DistOfCustByCountry.GetCustomersCountByCountry", "Plugins/Tutorial/DistOfCustByCountry/GetCustomersCountByCountry",
            //    new { controller = "DistOfCustByCountry", action = "GetCustomersCountByCountry", area = AreaNames.Admin });

            endpointRouteBuilder.MapControllerRoute("Plugins.Tutorial.DistOfCustByCountry.Configure", "Admin/Report/DistOfCustByCountry",
                new { controller = "DistOfCustByCountry", action = "Configure", area = AreaNames.Admin });

            endpointRouteBuilder.MapControllerRoute("Plugin.Tutorial.DistOfCustByCountry.GetCustomersCountByCountry", "Admin/Report/DistOfCustByCountry/GetCustomersCountByCountry",
                new { controller = "DistOfCustByCountry", action = "GetCustomersCountByCountry", area = AreaNames.Admin });

        }

        /// <summary>
        /// Gets a priority of route provider
        /// </summary>
        public int Priority => 1; //set a value that is greater than the default one in Nop.Web to override routes
    }
}