using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Quanda.Client.Services;
using Quanda.Client.Shared.RightMenu;

namespace Quanda.Client
{
    public partial class App
    {
        /// <summary>
        ///     Odnośnik do serwisu prawego menu
        /// </summary>
        [Inject]
        public RightMenuStateService RightMenuStateService { get; set; }

        public void NavigateHandler(NavigationContext navContext)
        {
            RightMenuStateService.RightMenuType = RightMenuType.NONE;
        }
    }
}
