using Microsoft.AspNetCore.Components;
using Quanda.Client.Services;

namespace Quanda.Client.Shared.RightMenu
{
    public partial class RightMenu
    {
        /// <summary>
        ///     Odnośnik do stanu prawego menu
        /// </summary>
        /// 
        [Inject]
        public RightMenuStateService RightMenuStateService { get; set; }

        protected override void OnInitialized()
        {
            RightMenuStateService.OnAction += StateHasChanged;
        }

        public void Dispose()
        {
            RightMenuStateService.OnAction -= StateHasChanged;
        }
    }
}
