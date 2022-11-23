using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Core;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Tutorial.DistOfCustByCountry.Models;
using Nop.Plugin.Tutorial.DistOfCustByCountry.Services;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Models.DataTables;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Tutorial.DistOfCustByCountry.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    public class DistOfCustByCountryController : BasePluginController
    {
      
        private readonly ICustomersByCountry _customersByCountry;

        public DistOfCustByCountryController(ICustomersByCountry customersByCountry  )
        {
            _customersByCountry = customersByCountry;
        }

        [HttpGet]
        public IActionResult Configure()
        {
            CustomersByCountrySearchModel customerSearchModel = new CustomersByCountrySearchModel
            {
                AvailablePageSizes = "10"
            };
            return View("~/Plugins/Tutorial.DistOfCustByCountry/Views/Configure.cshtml", customerSearchModel);
        }

        [HttpPost]
        public async Task<IActionResult> GetCustomersCountByCountry(CustomersByCountrySearchModel searchModel)
        {
            try
            {
                var model = await _customersByCountry.GetCustomersDistributionByCountryAsync(searchModel);
                return Json(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}