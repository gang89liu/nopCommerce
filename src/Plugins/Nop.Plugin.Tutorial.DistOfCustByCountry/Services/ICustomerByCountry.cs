using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.Tutorial.DistOfCustByCountry.Models;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Tutorial.DistOfCustByCountry.Services
{
    public interface ICustomersByCountry
    {
        Task<CustomersByCountryPageModel> GetCustomersDistributionByCountryAsync(CustomersByCountrySearchModel searchModel);
    }
}
