using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Quanda.Client.Shared.RightMenu
{
    public partial class RightShared
    {
        // <summary>
        ///     Zmienna która mówi jakie menu prawe wybrano
        /// </summary>
        [Parameter] 
        public RightMenuType RightMenuType { get; set; }
    }
}
