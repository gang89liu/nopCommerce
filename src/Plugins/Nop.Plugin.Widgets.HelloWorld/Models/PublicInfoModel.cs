using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.HelloWorld.Models
{
    public record PublicInfoModel : BaseNopModel
    {
        public string DisplayText { get; set; }
    }
}
