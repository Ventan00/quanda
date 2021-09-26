using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Quanda.Client.Services;
using Quanda.Client.Shared.RightMenu;

namespace Quanda.Client.Pages
{
    public partial class Home
    {
        /// <summary>
        ///     Zmienna przechowująca stan autoryzacji użytkownika
        /// </summary>
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        /// <summary>
        ///     Zmienna która jest odnośnikiem do serwisu prawego menu
        /// </summary>
        [Inject]
        public RightMenuStateService RightMenuStateService { get; set; }

        public ClaimsPrincipal user;

        protected async override Task OnParametersSetAsync()
        {
            var authState = await authenticationStateTask;
            user = authState.User;
            if (user.Identity.IsAuthenticated)
            {
                RightMenuStateService.RightMenuType = RightMenuType.STANDARD;
            }
        }

    }
}
