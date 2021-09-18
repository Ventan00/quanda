using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Quanda.Client.Services;
using Quanda.Client.Shared.RightMenu;

namespace Quanda.Client.Pages
{
    public partial class Question
    {
        [Parameter]
        public int IdQuestion { get; set; }

        /// <summary>
        ///     Zmienna która jest callbackiem main layoutu informująca czy i jakie prawe menu powinno być pokazane
        /// </summary>
        [Inject]
        public RightMenuStateService RightMenuStateService { get; set; }

        protected async override Task OnParametersSetAsync()
        {
            RightMenuStateService.RightMenuType = RightMenuType.STANDARD;
            await base.OnParametersSetAsync();
        }

    }
}
