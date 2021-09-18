using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Quanda.Client.Services;
using Quanda.Client.Shared.RightMenu;

namespace Quanda.Client.Shared
{
    public partial class MainLayout
    {
        /// <summary>
        ///     Zmienna opisująca jaki jest theme
        /// </summary>
        public bool isWhiteTheme = true;

        /// <summary>
        ///     Odnośnik do stanu prawego menu
        /// </summary>
        [Inject]
        public RightMenuStateService RightMenuStateService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _interceptorService.RegisterEvents();
        }


        public void Dispose()
        {
            _interceptorService.DisposeEvents();
        }

        /// <summary>
        ///     Funkcja która zmienia aktualny theme
        /// </summary>
        /// <param name="theme">True = light, False = dark</param>
        /// <returns>void</returns>
        public async Task HandleThemeChange(bool theme)
        {
            isWhiteTheme = theme;
        }
    }
}
