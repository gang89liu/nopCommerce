using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Tutorial.DistOfCustByCountry.Models
{
    public record CustomersDistribution : BaseNopModel
    {
        /// <summary>
        /// Country based on the billing address.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Number of customers from specific country.
        /// </summary>
        public int NoOfCustomers { get; set; }
    }
}
