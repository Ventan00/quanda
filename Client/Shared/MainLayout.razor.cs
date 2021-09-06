using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
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
        ///     Zmienna która opisuje czy na danej stronie ma wygenerować prawe menu, a jeśli tak to jakie
        /// </summary>
        public RightMenuType selectedMenu = RightMenuType.NONE;

        /// <summary>
        ///     Zmienna callbackowa służąca do ustawienia widoczności prawego menu
        /// </summary>
        public EventCallback<RightMenuType> callbackToRightMenuComponetAsValue;

        /// <summary>
        ///     Callback zmieniający widoczność prawego menu
        /// </summary>
        /// <param name="menuType"></param>
        public void changeMenuType(RightMenuType menuType)
        {
            selectedMenu = menuType;
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            callbackToRightMenuComponetAsValue = new EventCallback<RightMenuType>(this, (Action<RightMenuType>)changeMenuType);
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
