using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Quanda.Client.Components
{
    public partial class CaptchaComponent : IAsyncDisposable
    {
        /// <summary>
        ///     Captcha container (div) reference
        /// </summary>
        private ElementReference _captchaElementRef;

        /// <summary>
        ///     Captcha Id generated after successful render
        /// </summary>
        private int? _widgetUniqueId;

        /// <summary>
        ///     JsModule reference witch captcha functions
        /// </summary>
        private IJSObjectReference _captchaJsModule;

        [Inject] public IJSRuntime JsRuntime { get; set; }

        /// <summary>
        ///     Method that renders captcha
        /// </summary>
        /// <param name="firstRender"></param>
        /// <returns></returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
                return;

            _captchaJsModule = await JsRuntime.InvokeAsync<IJSObjectReference>
                ("import", "./js/ReCaptcha.js").AsTask();

            const string SiteKey = "6LcAdJYbAAAAAPKLVRM3QTVqqv05ylLQO03sgX3w";
            await _captchaJsModule.InvokeVoidAsync("tryRender",
                DotNetObjectReference.Create(this), _captchaElementRef, SiteKey);
        }

        /// <summary>
        /// Callback called from CaptchaJsModule after successful render
        /// </summary>
        /// <param name="widgetId">Captcha unique widgetId returned from grecaptcha.render</param>
        [JSInvokable]
        public void OnWidgetIdSet(int widgetId)
        {
            _widgetUniqueId = widgetId;
        }

        /// <summary>
        ///     Method called for getting captcha response
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetCaptchaResponseAsync()
        {
            if (_widgetUniqueId is null) return null;

            try
            {
                var responseToken = await _captchaJsModule.InvokeAsync<string>("getResponse", _widgetUniqueId);
                return responseToken.Length <= 0 ? null : responseToken;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///     Method called to reset captcha
        /// </summary>
        /// <returns></returns>
        public async Task ResetCaptchaAsync()
        {
            if (_widgetUniqueId is null)
                return;

            await _captchaJsModule.InvokeVoidAsync("reset", _widgetUniqueId);
        }

        /// <summary>
        ///     Method called after component dispose in order to delete captcha data
        /// </summary>
        /// <returns></returns>
        public async ValueTask DisposeAsync()
        {
            await ResetCaptchaAsync();
            await _captchaJsModule.DisposeAsync();
        }
    }
}
