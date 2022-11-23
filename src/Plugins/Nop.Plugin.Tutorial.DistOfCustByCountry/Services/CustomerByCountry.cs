using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.Tutorial.DistOfCustByCountry.Models;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Tutorial.DistOfCustByCountry.Services
{
    public class CustomersByCountry : ICustomersByCountry
    {
        private readonly IAddressService _addressService;
        private readonly ICountryService _countryService;
        private readonly ICustomerService _customerService;

        public CustomersByCountry(IAddressService addressService
            , ICountryService countryService
            , ICustomerService customerService)
        {
            _addressService = addressService;
            _countryService = countryService;
            _customerService = customerService;
        }
        public async Task<List<CustomersDistribution>> GetCustomersDistributionByCountryAsync()
        {
            return
                   (await _customerService.GetAllCustomersAsync())
                     .Where(c => c.ShippingAddressId != null)
                     .Select
                     (c => new
                     {
                         Name = _countryService.GetCountryByAddressAsync(_addressService.GetAddressByIdAsync(c.ShippingAddressId ?? 0)
                                 .GetAwaiter().GetResult()).GetAwaiter().GetResult().Name,
                         c.Username
                     })
                     .GroupBy(c => c.Name)
                     .Select(cbc => new CustomersDistribution { Country = cbc.Key, NoOfCustomers = cbc.Count() }).ToList();
        }
    }
}
