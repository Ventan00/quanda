using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Quanda.Client.Components;
using Quanda.Client.Extensions;
using Quanda.Client.Shared.RightMenu;
using Quanda.Shared.DTOs.Requests;

namespace Quanda.Client.Pages.Recover
{
    public partial class ResetPassword
    {
        /// <summary>
        ///     Zmienna która jest callbackiem main layoutu informująca czy i jakie prawe menu powinno być pokazane
        /// </summary>
        [CascadingParameter]
        public EventCallback<RightMenuType> RightMenuTypeCallback { get; set; }
        protected async override Task OnInitializedAsync()
        {
            await RightMenuTypeCallback.InvokeAsync(RightMenuType.NONE);
        }

        /// <summary>
        ///     Model formularza
        /// </summary>
        private PasswordResetDTO _passwordResetDto;

        /// <summary>
        ///     Zmienna mowiąca o tym, czy formularz jest w trakcie przetwarzania
        /// </summary>
        private bool _isPending;

        /// <summary>
        ///     Referencja do komponentu formularza
        /// </summary>
        private GenericFormComponent<PasswordResetDTO> _form;

        /// <summary>
        ///     Metoda wywoływana przy 1 renderowaniu komponentu
        /// </summary>
        protected override void OnInitialized()
        {
            VerifyQueryParams();
        }

        /// <summary>
        ///     Metoda sprawdzajaca poprawnosc query parametrów z adresu URL
        /// </summary>
        private void VerifyQueryParams()
        {
            var uuidFromQuery = _navManager.ExtractQueryStringByKey<int?>("uuid");
            var recoveryTokenFromQuery = _navManager.ExtractQueryStringByKey<string>("recovery_token");

            if (uuidFromQuery is null || string.IsNullOrEmpty(recoveryTokenFromQuery))
            {
                _navManager.NavigateTo("/recover/confirmation-email");
                _toastService.ShowError("Password reset link is invalid. Please try recovering your password again.");
                return;
            }

            _passwordResetDto = new PasswordResetDTO
            {
                IdUser = (int)uuidFromQuery,
                UrlEncodedRecoveryJwt = recoveryTokenFromQuery
            };
        }

        /// <summary>
        ///     Metoda wysyłająca oraz obsługująca żądanie ustawienia nowego hasła - wywoływana przy potwierdzeniu prawidłowo
        ///     wypełnionego formularza
        /// </summary>
        /// <returns></returns>
        private async Task ResetPasswordAsync()
        {
            _isPending = true;
            _passwordResetDto.CaptchaResponseToken = await _form.CaptchaComponent.GetCaptchaResponseAsync();

            var responseStatusCode = await _usersRepository.ResetPasswordAsync(_passwordResetDto);
            switch (responseStatusCode)
            {
                case HttpStatusCode.NoContent:
                    _toastService.ShowSuccess("Your password has been changed.");
                    _navManager.NavigateTo("/login");
                    return;
                case HttpStatusCode.Unauthorized:
                    _toastService.ShowError("reCaptcha must be completed.");
                    break;
                case HttpStatusCode.Forbidden:
                    _toastService.ShowError("Something is wrong. Please recover your password again.");
                    _navManager.NavigateTo("/recover/password");
                    return;
            }

            await _form.CaptchaComponent.ResetCaptchaAsync();
            _isPending = false;
        }
    }
}
